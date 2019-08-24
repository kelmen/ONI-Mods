﻿using KSerialization;
using System;
using UnityEngine;

namespace Kelmen.ONI.Mods.ConduitFilters.TemperatureFilters
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public partial class TemperatureFilterProcess : KMonoBehaviour, ISaveLoadable, ISecondaryOutput
    {
        int InputCell;
        int OutputCell1;
        int OutputCell2;
        FlowUtilityNetwork.NetworkItem OutputItem2;

        [SerializeField]
        public ConduitPortInfo OutputPort2Info;

        ConduitType? _ConduitType = null;
        ConduitType ConduitType
        {
            get
            {
                if (_ConduitType == null)
                    _ConduitType = this.OutputPort2Info.conduitType;

                return _ConduitType.Value;
            }
        }

        Guid ConduitBlockedStatusItemGuid = Guid.Empty;

        [MyCmpReq]
        Building Building = null;

        //[MyCmpReq]
        //KSelectable Selectable = null;

        [MyCmpReq]
        Operational Operation = null;

        //[SerializeField]
        //[MyCmpReq]
        //public TemperatureFilterData FilterData;

        Guid OutputConduit2StatusGuid = Guid.Empty;

        ConduitFlow _FlowMgr = null;
        ConduitFlow FlowMgr
        {
            get
            {
                if (_FlowMgr == null)
                    _FlowMgr = Conduit.GetFlowManager(this.ConduitType);

                return _FlowMgr;
            }
        }

        IUtilityNetworkMgr _NetworkMgr = null;
        IUtilityNetworkMgr NetworkMgr
        {
            get
            {
                if (_NetworkMgr == null)
                    _NetworkMgr = Conduit.GetNetworkManager(this.ConduitType); ;

                return _NetworkMgr;
            }
        }

        float _ConduitMassMax = -1;
        float ConduitMassMax
        {
            get
            {
                if (_ConduitMassMax < 0)
                    switch (this.ConduitType)
                    {
                        case ConduitType.Gas:
                            _ConduitMassMax = 1;
                            break;
                        case ConduitType.Liquid:
                            _ConduitMassMax = 10;
                            break;
                        default:
                            _ConduitMassMax = 0;
                            break;
                    }

                return _ConduitMassMax;
            }
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            Validate();

            InputCell = this.Building.GetUtilityInputCell();
            OutputCell1 = this.Building.GetUtilityOutputCell();
            OutputCell2 = Grid.OffsetCell(Grid.PosToCell(transform.GetPosition()), this.Building.GetRotatedOffset(OutputPort2Info.offset));

            OutputItem2 = new FlowUtilityNetwork.NetworkItem(this.ConduitType, Endpoint.Source, OutputCell2, gameObject);

            NetworkMgr.AddToNetworks(OutputCell2, OutputItem2, true);

            this.GetComponent<ConduitConsumer>().isConsuming = false;

            FlowMgr.AddConduitUpdater(OnConduitUpdate);

            //this.UpdateBuildingStatus();
            //this.UpdateConduitBlockedStatus();
        }

        protected override void OnCleanUp()
        {
            FlowMgr.RemoveConduitUpdater(this.OnConduitUpdate);
            NetworkMgr.RemoveFromNetworks(this.OutputCell2, this.OutputItem2, true);

            base.OnCleanUp();
        }

        void OnConduitUpdate(float data)
        {
            //bool setActive = false;
            //this.UpdateConduitBlockedStatus();

            if (!this.Operation.IsOperational)
                return;

            var inputContent = FlowMgr.GetContents(InputCell);
            if (inputContent.mass > 0f)
            {
                int outputCellIdx;
                var filterData = this;

                if (filterData.ActivateAboveThreshold)
                    outputCellIdx = inputContent.temperature < filterData.Threshold ? this.OutputCell1 : this.OutputCell2;
                else
                    outputCellIdx = inputContent.temperature > filterData.Threshold ? this.OutputCell1 : this.OutputCell2;

                //bool elementMovedWhole = true;
                float massMove = inputContent.mass;
                int diseaseCount = inputContent.diseaseCount;

                if (!FlowMgr.IsConduitEmpty(outputCellIdx))
                {
                    var outputContent = FlowMgr.GetContents(outputCellIdx);
                    if ((outputContent.mass >= this.ConduitMassMax)
                        || (!inputContent.element.Equals(outputContent.element))
                        //|| (inputContent.diseaseIdx != outputContent.diseaseIdx)
                        )
                        return;

                    //if (inputContent.diseaseIdx == outputContent.diseaseIdx)
                    //    diseaseCount += outputContent.diseaseCount;

                    if ((inputContent.mass + outputContent.mass) > this.ConduitMassMax)
                    {
                        //elementMovedWhole = false;
                        massMove = this.ConduitMassMax - outputContent.mass;
                    }

                    if (diseaseCount > 0)
                    {
                        var ratioMove = massMove / inputContent.mass;
                        diseaseCount = (int)(diseaseCount * ratioMove);
                    }

                    //FlowMgr.RemoveElement(outputCellIdx, outputContent.mass);
                }

                float elementMoved = FlowMgr.AddElement(outputCellIdx, inputContent.element, massMove, inputContent.temperature, inputContent.diseaseIdx, diseaseCount);
                if (elementMoved > 0f)
                    FlowMgr.RemoveElement(this.InputCell, elementMoved);
            }

            //this.Operation.SetActive(setActive, false);
        }

        //void UpdateConduitBlockedStatus()
        //{
        //    bool outputConduit2IsEmpty = FlowMgr.IsConduitEmpty(this.OutputCell2);
        //    bool blockageDetected = (this.ConduitBlockedStatusItemGuid != Guid.Empty);
        //    if (outputConduit2IsEmpty != blockageDetected)
        //        return;

        //    StatusItem blockedMultiples = Db.Get().BuildingStatusItems.ConduitBlockedMultiples;
        //    this.ConduitBlockedStatusItemGuid = this.Selectable.ToggleStatusItem(blockedMultiples, this.ConduitBlockedStatusItemGuid, !outputConduit2IsEmpty);
        //}

        //void UpdateBuildingStatus()
        //{
        //    bool outputCell2IsConnected = RequireOutputs.IsConnected(this.OutputCell2, this.ConduitType);
        //    StatusItem outputConduit2Status;

        //    var buildingStatus = Db.Get().BuildingStatusItems;
        //    switch (this.OutputPort2Info.conduitType)
        //    {
        //        case ConduitType.Gas:
        //            outputConduit2Status = buildingStatus.NeedGasOut;
        //            break;
        //        case ConduitType.Liquid:
        //            outputConduit2Status = buildingStatus.NeedLiquidOut;
        //            break;
        //        case ConduitType.Solid:
        //            outputConduit2Status = buildingStatus.NeedSolidOut;
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //    bool outputConduit2StatusIsEmpty = this.OutputConduit2StatusGuid != Guid.Empty;
        //    if (outputCell2IsConnected != outputConduit2StatusIsEmpty)
        //    {
        //        return;
        //    }
        //    this.OutputConduit2StatusGuid = this.Selectable.ToggleStatusItem(outputConduit2Status, this.OutputConduit2StatusGuid, !outputCell2IsConnected);
        //}

        void Validate()
        {
            string msg = null;
            switch (this.OutputPort2Info.conduitType)
            {
                case ConduitType.Gas:
                case ConduitType.Liquid:
                case ConduitType.Solid:
                    break;

                default:
                    msg = $"Output conduit 2 type: {this.OutputPort2Info.conduitType} is not supported.";
                    throw new ArgumentException(msg);
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