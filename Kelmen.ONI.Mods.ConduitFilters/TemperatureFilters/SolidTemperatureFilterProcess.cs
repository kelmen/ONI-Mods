using KSerialization;
using System;
using UnityEngine;

using KelmenUtils = Kelmen.ONI.Mods.ConduitFilters.Utils;

namespace Kelmen.ONI.Mods.ConduitFilters.TemperatureFilters
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class SolidTemperatureFilterProcess : TemperatureFilterData, ISaveLoadable, ISecondaryOutput
    {
        int OutputCell1;
        int OutputCell2;
        FlowUtilityNetwork.NetworkItem OutputItem2;

        readonly ConduitType ConduitType = ConduitType.Solid;

        [MyCmpReq]
        Building Building = null;

        [MyCmpReq]
        Operational Operation = null;

        SolidConduitFlow _FlowMgr = null;
        SolidConduitFlow FlowMgr 
        {
            get
            {
                if (_FlowMgr == null)
                    _FlowMgr = SolidConduit.GetFlowManager();

                return _FlowMgr;
            }
        }

        UtilityNetworkManager<FlowUtilityNetwork, SolidConduit> _NetworkMgr = null;
        UtilityNetworkManager<FlowUtilityNetwork, SolidConduit> NetworkMgr
        {
            get
            {
                if (_NetworkMgr == null)
                    _NetworkMgr = Game.Instance.solidConduitSystem;

                return _NetworkMgr;
            }
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
        }

        protected override void OnSpawn()
        {
            try
            {
                base.OnSpawn();

                InputCell = Building.GetUtilityInputCell();

                OutputCell1 = Building.GetUtilityOutputCell();

                OutputCell2 = Grid.OffsetCell(Grid.PosToCell(transform.GetPosition()), Building.GetRotatedOffset(OutputPort2Info.offset));

                OutputItem2 = new FlowUtilityNetwork.NetworkItem(this.ConduitType, Endpoint.Source, OutputCell2, gameObject);

                NetworkMgr.AddToNetworks(OutputCell2, OutputItem2, true);

                FlowMgr.AddConduitUpdater(OnConduitUpdate);
            }
            catch (Exception ex)
            {
                KelmenUtils.Log("SolidTemperatureFilterProcess.OnSpawn", ex);
                throw ex;
            }
        }

        protected override void OnCleanUp()
        {
            FlowMgr.RemoveConduitUpdater(this.OnConduitUpdate);
            NetworkMgr.RemoveFromNetworks(this.OutputCell2, this.OutputItem2, true);

            base.OnCleanUp();
        }

        void OnConduitUpdate(float data)
        {
            try
            {
                if (!this.Operation.IsOperational) return;

                if (FlowMgr.HasConduit(InputCell) && FlowMgr.HasConduit(OutputCell1) && FlowMgr.HasConduit(OutputCell2)
                    &&
                    FlowMgr.IsConduitFull(InputCell) 
                    //&& FlowMgr.IsConduitEmpty(OutputCell1) && FlowMgr.IsConduitEmpty(OutputCell2)
                    )
                { }
                else
                    return;

                var inputContent = FlowMgr.GetPickupable(FlowMgr.GetContents(InputCell).pickupableHandle);
                //var inputContent = FlowMgr.RemovePickupable(InputCell);
                if (inputContent == null)
                    return;

                var inputContent2 = inputContent.PrimaryElement;
                if (inputContent2 == null)
                    return;

                var filterData = this;
                int outputCellIdx = filterData.GetOutputRouteIdx(inputContent2.Temperature, this.OutputCell1, this.OutputCell2);

                if (outputCellIdx == OutputCell1) 
                    if (FlowMgr.IsConduitFull(OutputCell1)) return;
                else //if (outputCellIdx == OutputCell2)
                    if (FlowMgr.IsConduitFull(OutputCell2)) return;

                //var outputTgt = FlowMgr.GetContents(outputCellIdx);
                //if (outputTgt.pickupableHandle.IsValid())
                //    return;

                inputContent = FlowMgr.RemovePickupable(InputCell);
                FlowMgr.AddPickupable(outputCellIdx, inputContent);

                this.Operation.SetActive(false);
            }
            catch (Exception ex)
            {
                KelmenUtils.Log("SolidTemperatureFilterProcess.OnConduitUpdate", ex);
                throw ex;
            }
        }

        #region ISecondaryOutput

        public ConduitType GetSecondaryConduitType()
        {
            return this.ConduitType;
        }

        public CellOffset GetSecondaryConduitOffset()
        {
            return this.OutputPort2Info.offset;
        }

        #endregion

    }
}
