using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Kelmen.ONI.Mods.TemperatureFilterPipe
{
    public class Process : KMonoBehaviour, IBridgedNetworkItem, ISecondaryOutput
    {
        [SerializeField]
        public ConduitPortInfo OutputPort2;

        [SerializeField]
        public ConduitType PipingType;

        public enum TemperatureConditionOperators
        {
            NoCondition = 0,
            LessThan = 1,
            LessThanOrEqual = 2,
            Equal = 3,
            EqualOrGreaterThan = 4,
            GreaterThan = 5,
        }

        CfgData ProcessData;

        HandleVector<int>.Handle Accumulator = HandleVector<int>.InvalidHandle;
        int InputCell;
        int OutputCell1;
        int OutputCell2;
        FlowUtilityNetwork.NetworkItem OutputItem2;

        ConduitFlow _FlowMgr = null;
        ConduitFlow FlowMgr
        {
            get
            {
                if (_FlowMgr == null)
                    _FlowMgr = Conduit.GetFlowManager(PipingType);

                return _FlowMgr;
            }
        }

        IUtilityNetworkMgr _NetworkMgr = null;
        IUtilityNetworkMgr NetworkMgr
        {
            get
            {
                if (_NetworkMgr == null)
                    _NetworkMgr = Conduit.GetNetworkManager(PipingType); ;

                return _NetworkMgr;
            }
        }

        //float PipingMassMax = 0f;

        const string AccumulatorName = "Flow";

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            Accumulator = Game.Instance.accumulators.Add(AccumulatorName, this);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            if (_IsInitValidated == null)
                InitValidate();

            if (!_IsInitValidated.GetValueOrDefault(false))
                //return;
                throw new Exception(Message);

            LoadParameters();

            //Utils.Log($"TemperatureConditionOperator {this.TemperatureConditionOperator}");
            //Utils.Log($"TemperatureConditionValue {this.TemperatureConditionValue}");

            //if (Accumulator == HandleVector<int>.InvalidHandle)
            //    Accumulator = Game.Instance.accumulators.Add(AccumulatorName, this);

            var building = GetComponent<Building>();
            InputCell = building.GetUtilityInputCell();
            OutputCell1 = building.GetUtilityOutputCell();
            OutputCell2 = Grid.OffsetCell(Grid.PosToCell(transform.GetPosition()), building.GetRotatedOffset(OutputPort2.offset));
            //OutputItem2 = new FlowUtilityNetwork.NetworkItem(OutputPort2.conduitType, Endpoint.Source, OutputCell2, UnityEngine.Component.gameObject);
            OutputItem2 = new FlowUtilityNetwork.NetworkItem(OutputPort2.conduitType, Endpoint.Source, OutputCell2, gameObject);

            var networkManager = Conduit.GetNetworkManager(OutputPort2.conduitType);
            networkManager.AddToNetworks(OutputCell2, OutputItem2, true);

            Conduit.GetFlowManager(PipingType).AddConduitUpdater(Updater);
        }

        protected override void OnCleanUp()
        {
            Conduit.GetNetworkManager(PipingType).RemoveFromNetworks(OutputCell2, OutputItem2, true);
            Conduit.GetFlowManager(PipingType).RemoveConduitUpdater(Updater);

            Game.Instance.accumulators.Remove(Accumulator);

            base.OnCleanUp();
        }

        #region IBridgedNetworkItem

        public void AddNetworks(ICollection<UtilityNetwork> networks)
        {
            var networkForInput = NetworkMgr.GetNetworkForCell(InputCell);
            if (networkForInput != null)
                networks.Add(networkForInput);

            var networkForOutputCell1 = NetworkMgr.GetNetworkForCell(OutputCell1);
            if (networkForOutputCell1 != null)
                networks.Add(networkForOutputCell1);

            var networkForOutputCell2 = NetworkMgr.GetNetworkForCell(OutputCell2);
            if (networkForOutputCell2 != null)
                networks.Add(networkForOutputCell2);
        }

        public int GetNetworkCell()
        {
            return InputCell;
        }

        public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
        {
            return networks.Contains(NetworkMgr.GetNetworkForCell(InputCell)) 
                || networks.Contains(NetworkMgr.GetNetworkForCell(OutputCell1))                   
                || networks.Contains(NetworkMgr.GetNetworkForCell(OutputCell2));
        }

        #endregion

        #region ISecondaryOutput

        public CellOffset GetSecondaryConduitOffset()
        {
            return OutputPort2.offset;
        }

        public ConduitType GetSecondaryConduitType()
        {
            return OutputPort2.conduitType;
        }

        #endregion

        public string Message { get; private set; }

        bool? _IsInitValidated = null;
        public bool InitValidate()
        {
            _IsInitValidated = true;

            //switch (PipingType)
            //{
            //    case ConduitType.Liquid:
            //        PipingMassMax = 10f;
            //        _IsInitValidated = true;
            //        break;

            //    case ConduitType.Gas:
            //        PipingMassMax = 1f;
            //        _IsInitValidated = true;
            //        break;

            //    default:
            //        var sb = new StringBuilder(Message);
            //        //sb.AppendLine($"{PipingType} is not supported.");
            //        Message = sb.AppendLine($"{PipingType} is not supported.").ToString();
            //        _IsInitValidated = false;
            //        break;
            //}

            return _IsInitValidated.Value;
        }

        void Updater(float data)
        {
            if (!IsOperational)
                return;

            var contents = FlowMgr.GetContents(InputCell);
            if (contents.mass <= 0)
                return;

            float delta1 = 0;
            float delta2 = 0;

            if (EvaluateTemperate(contents.temperature))
            {
                delta2 = FlowMgr.AddElement(OutputCell2, contents.element, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount);
            }
            else
            {
                delta1 = FlowMgr.AddElement(OutputCell1, contents.element, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount);
            }

            FlowMgr.RemoveElement(InputCell, delta1);
            FlowMgr.RemoveElement(InputCell, delta2);

            Game.Instance.accumulators.Accumulate(Accumulator, contents.mass);
        }

        bool IsOperational
        {
            get
            {
                //var flowManager = Conduit.GetFlowManager(PipingType);
                return FlowMgr.HasConduit(InputCell) || FlowMgr.HasConduit(OutputCell1) || FlowMgr.HasConduit(OutputCell2);
            }
        }

        void LoadParameters()
        {
            // todo
            //TemperatureConditionOperator = TemperatureConditionOperators.GreaterThan;
            //TemperatureConditionValue = 293.15f; // 20 C

            if (ProcessData == null)
                ProcessData = new CfgData();
        }

        bool EvaluateTemperate(float temperature)
        {
            switch (ProcessData.TemperatureConditionOperator)
            {
                case TemperatureConditionOperatorControl.ConditionOperators.LessThan:
                    return temperature < ProcessData.TemperatureConditionValue;

                case TemperatureConditionOperatorControl.ConditionOperators.LessThanOrEqual:
                    return temperature <= ProcessData.TemperatureConditionValue;

                case TemperatureConditionOperatorControl.ConditionOperators.Equal:
                    return temperature == ProcessData.TemperatureConditionValue;

                case TemperatureConditionOperatorControl.ConditionOperators.EqualOrGreaterThan:
                    return temperature >= ProcessData.TemperatureConditionValue;

                case TemperatureConditionOperatorControl.ConditionOperators.GreaterThan:
                    return temperature > ProcessData.TemperatureConditionValue;
            }

            return false;
        }
    }
}
