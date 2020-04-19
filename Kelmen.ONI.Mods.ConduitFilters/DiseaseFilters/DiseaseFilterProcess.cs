using KSerialization;
using System;
using UnityEngine;

using KelmenUtils = Kelmen.ONI.Mods.ConduitFilters.Utils;

namespace Kelmen.ONI.Mods.ConduitFilters.DiseaseFilters
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class DiseaseFilterProcess : KMonoBehaviour, ISaveLoadable, ISecondaryOutput
    {
        [MyCmpReq]
        Building Building = null;

        [MyCmpReq]
        Operational Operation = null;

        [MyCmpReq]
        KSelectable Selectable = null;

        protected int InputCell;
        int OutputCell1;
        int OutputCell2;
        FlowUtilityNetwork.NetworkItem OutputItem2;

        Guid ConduitBlockedStatusItemGuid = Guid.Empty;

        //byte MinDiseaseIdx = 0;

        [SerializeField]
        public ConduitPortInfo OutputPort2Info;

        #region ConduitType

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

        #endregion

        #region NetworkMgr

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

        #endregion

        #region FlowMgr

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

        #endregion

        #region ConduitMassMax

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
                        case ConduitType.Solid:
                            _ConduitMassMax = 20; // Conveyor Rail
                            break;
                        default:
                            _ConduitMassMax = 0;
                            break;
                    }

                return _ConduitMassMax;
            }
        }

        #endregion


        protected override void OnSpawn()
        {
            try
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

                #region no idea what these do, just copy from game codes ElementFilter.OnSpawn()

                //((KSelectable)((Component)this).GetComponent<KSelectable>()).SetStatusItem(Db.Get().StatusItemCategories.Main, TemperatureFilterProcess.filterStatusItem, (object)this);

                //this.UpdateConduitExistsStatus();
                this.UpdateConduitBlockedStatus();

                //ScenePartitionerLayer layer = (ScenePartitionerLayer)null;
                //switch (this.ConduitType)
                //{
                //    case ConduitType.Gas:
                //        layer = GameScenePartitioner.Instance.gasConduitsLayer;
                //        break;
                //    case ConduitType.Liquid:
                //        layer = GameScenePartitioner.Instance.liquidConduitsLayer;
                //        break;
                //    case ConduitType.Solid:
                //        layer = GameScenePartitioner.Instance.solidConduitsLayer;
                //        break;
                //}
                //if (layer == null)
                //    return;

                //this.partitionerEntry = GameScenePartitioner.Instance.Add("TemperatureFilterProcessConduitExists", this.gameObject, OutputCell2, layer, (System.Action<object>)(data => this.UpdateConduitExistsStatus()));

                #endregion
            }
            catch (Exception ex)
            {
                KelmenUtils.Log("DiseaseFilterProcess.OnSpawn", ex);
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

        void UpdateConduitBlockedStatus()
        {
            bool outputConduit2IsEmpty = FlowMgr.IsConduitEmpty(this.OutputCell2);
            bool blockageDetected = (this.ConduitBlockedStatusItemGuid != Guid.Empty);
            if (outputConduit2IsEmpty != blockageDetected)
                return;

            StatusItem blockedMultiples = Db.Get().BuildingStatusItems.ConduitBlockedMultiples;
            this.ConduitBlockedStatusItemGuid = this.Selectable.ToggleStatusItem(blockedMultiples, this.ConduitBlockedStatusItemGuid, !outputConduit2IsEmpty);
        }

        bool IsDisease(byte diseaseIdx)
        {
            return (diseaseIdx != byte.MaxValue);
        }

        int GetOutputRouteIdx(byte diseaseIdx, int diseaseCount, int outputRoute1Idx, int outputRoute2Idx)
        {
            return (
                //(diseaseIdx >= MinDiseaseIdx) &&
                IsDisease(diseaseIdx) && (diseaseCount > 0)) ? outputRoute2Idx : outputRoute1Idx;
        }

        void OnConduitUpdate(float data)
        {
            try
            {
                bool setActive = false;
                this.UpdateConduitBlockedStatus();

                if (this.Operation.IsOperational)
                {
                    var inputContent = FlowMgr.GetContents(InputCell);
                    if ((inputContent.mass > 0) && (inputContent.element != SimHashes.Vacuum))
                    {
                        int outputCellIdx = GetOutputRouteIdx(inputContent.diseaseIdx, inputContent.diseaseCount, this.OutputCell1, this.OutputCell2);

                        if (FlowMgr.IsConduitEmpty(outputCellIdx))
                        {
                            var outputContent = FlowMgr.GetContents(outputCellIdx);

                            /// ConduitFlow.AddElement() included these (similar) chks:
                            ///     (outputContent.mass >= this.ConduitMassMax)
                            ///     (!inputContent.element.Equals(outputContent.element))
                            ///     

                            float elementMoved = FlowMgr.AddElement(outputCellIdx
                                , inputContent.element, inputContent.mass, inputContent.temperature, inputContent.diseaseIdx, inputContent.diseaseCount);
                            
                            if (elementMoved > 0) FlowMgr.RemoveElement(this.InputCell, elementMoved);
                        }
                        //else
                        //{
                        //    /// partial transfer not catered in (core) ElementFilter.OnConduitTick, don't break core design.
                        //}
                    }
                }

                this.Operation.SetActive(setActive, false);
            }
            catch (Exception ex)
            {
                KelmenUtils.Log("DiseaseFilterProcess.OnConduitUpdate", ex);
                throw ex;
            }
        }

        protected override void OnCleanUp()
        {
            FlowMgr.RemoveConduitUpdater(this.OnConduitUpdate);
            NetworkMgr.RemoveFromNetworks(this.OutputCell2, this.OutputItem2, true);

            //if (this.partitionerEntry.IsValid() && (GameScenePartitioner.Instance != null))
            //    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);

            base.OnCleanUp();
        }

        void Validate()
        {
            string msg = null;
            switch (this.OutputPort2Info.conduitType)
            {
                case ConduitType.Gas:
                case ConduitType.Liquid:
                //case ConduitType.Solid:
                    break;

                default:
                    msg = $"Output conduit 2 type: {this.OutputPort2Info.conduitType} is not supported.";
                    throw new ArgumentException(msg);
            }
        }

    }
}
