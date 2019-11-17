using KSerialization;
using STRINGS;
using System;
using UnityEngine;

namespace Kelmen.ONI.Mods.Storages
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class HighInflowLiquidReservoirProcess : KMonoBehaviour//, ISecondaryInput
    {
        [SerializeField]
        public ConduitPortInfo InputPort2Info;

        //[SerializeField]
        public bool KeepZeroMassObject = true;


        #region InputPort2ConduitType
        ConduitType? _InputPort2ConduitType = null;
        ConduitType InputPort2ConduitType
        {
            get
            {
                if (_InputPort2ConduitType == null) _InputPort2ConduitType = InputPort2Info.conduitType;

                return _InputPort2ConduitType.Value;
            }
        }
        #endregion

        #region InputPort2Cell
        int? _InputPort2Cell = null;
        int InputPort2Cell
        {
            get
            {
                if (_InputPort2Cell == null)
                    _InputPort2Cell = Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), this.Building.GetRotatedOffset(this.InputPort2Info.offset));

                return _InputPort2Cell.Value;
            }
        }
        #endregion

        [MyCmpReq]
        protected Building Building;

        [MyCmpReq]
        public Operational Operational;

        [MyCmpGet]
        public Storage Storage;

        FlowUtilityNetwork.NetworkItem InputPort2NetworkItem;

        #region InputPort2NetworkMgr
        IUtilityNetworkMgr _InputPort2NetworkMgr = null;
        IUtilityNetworkMgr InputPort2NetworkMgr
        {
            get
            {
                if(_InputPort2NetworkMgr == null)
                    _InputPort2NetworkMgr = Conduit.GetNetworkManager(InputPort2ConduitType);

                return _InputPort2NetworkMgr;
            }
        }
        #endregion

        #region InputPort2ConduitFlow
        ConduitFlow _InputPort2ConduitFlow = null;
        ConduitFlow InputPort2ConduitFlow
        {
            get
            {
                if (_InputPort2ConduitFlow == null)
                {
                    switch (InputPort2ConduitType)
                    {
                        case ConduitType.Gas:
                            _InputPort2ConduitFlow = Game.Instance.gasConduitFlow;
                            break;
                        case ConduitType.Liquid:
                            _InputPort2ConduitFlow = Game.Instance.liquidConduitFlow;
                            break;
                        default:
                            //_InputPort2ConduitFlow = null;
                            //break;
                            throw new NotSupportedException("HighInflowLiquidReservoirProcess.InputPort2ConduitFlow type " + InputPort2ConduitType.ToString() + " not supported.");
                    }
                }

                return _InputPort2ConduitFlow;
            }
        }
        #endregion

        //protected HandleVector<int>.Handle InputPort2CellPartitionerEntry;

        //public bool IsInputPort2Connected
        //{
        //    get
        //    {
        //        GameObject gameObject = Grid.Objects[this.InputPort2Cell, this.InputPort2ConduitType != ConduitType.Gas ? 16 : 12];
        //        if (gameObject != null)
        //            return gameObject.GetComponent<BuildingComplete>() != null;

        //        return false;
        //    }
        //}

        ConduitFlow GetConduitManager()
        {
            switch (this.InputPort2ConduitType)
            {
                case ConduitType.Gas:
                    return Game.Instance.gasConduitFlow;
                case ConduitType.Liquid:
                    return Game.Instance.liquidConduitFlow;
                default:
                    return null;
            }
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            InputPort2NetworkItem = new FlowUtilityNetwork.NetworkItem(InputPort2ConduitType, Endpoint.Sink, this.InputPort2Cell, this.gameObject);

            InputPort2NetworkMgr.AddToNetworks(this.InputPort2Cell, InputPort2NetworkItem, true);

            //this.InputPort2CellPartitionerEntry = GameScenePartitioner.Instance.Add("HighInflowLiquidReservoirProcess.OnSpawn", this, InputPort2Cell
            //    , GameScenePartitioner.Instance.objectLayers[this.InputPort2ConduitType != ConduitType.Gas ? 16 : 12], new System.Action<object>(this.OnInputPort2ConduitConnectionChanged));

            this.InputPort2ConduitFlow.AddConduitUpdater(this.InputPort2ConduitUpdate, ConduitFlowPriority.Default);

            //this.OnInputPort2ConduitConnectionChanged(null);
        }

        //void OnInputPort2ConduitConnectionChanged(object data)
        //{
        //    this.Trigger(-2094018600, this.IsInputPort2Connected);
        //}

        protected override void OnCleanUp()
        {
            InputPort2ConduitFlow.RemoveConduitUpdater(this.InputPort2ConduitUpdate);
            //GameScenePartitioner.Instance.Free(ref this.InputPort2CellPartitionerEntry);

            InputPort2NetworkMgr.RemoveFromNetworks(this.InputPort2Cell, InputPort2NetworkItem, true);

            base.OnCleanUp();
        }

        //#region ISecondaryInput

        //CellOffset ISecondaryInput.GetSecondaryConduitOffset()
        //{
        //    return InputPort2Info.offset;
        //}

        //ConduitType ISecondaryInput.GetSecondaryConduitType()
        //{
        //    return InputPort2ConduitType;
        //}

        //#endregion

        public float StorageRemainingCapacity
        {
            get
            {
                return this.Storage.RemainingCapacity();
            }
        }

        void InputPort2ConduitUpdate(float dt)
        {
            if (!this.Operational.IsOperational)
                return;

            var conduitMgr = this.GetConduitManager();

            var input2Content = conduitMgr.GetContents(InputPort2Cell);

            float massMoved = Mathf.Min(input2Content.mass, this.StorageRemainingCapacity);
            if (massMoved <= 0) 
                return;

            ConduitFlow.ConduitContents conduitContents = conduitMgr.RemoveElement(this.InputPort2Cell, massMoved);

                
            int disease_count = (int)(input2Content.diseaseCount * (massMoved / input2Content.mass));
                
            switch (this.InputPort2ConduitType)
            {
                case ConduitType.Gas:
                        this.Storage.AddGasChunk(input2Content.element, massMoved, input2Content.temperature, input2Content.diseaseIdx, disease_count, this.KeepZeroMassObject, false);
                    break;
                case ConduitType.Liquid:
                        this.Storage.AddLiquid(input2Content.element, massMoved, input2Content.temperature, input2Content.diseaseIdx, disease_count, this.KeepZeroMassObject, false);
                    break;
            }
        }
    }
}
