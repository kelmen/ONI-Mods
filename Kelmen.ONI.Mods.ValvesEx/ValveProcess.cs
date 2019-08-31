using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Kelmen.ONI.Mods.ValvesEx
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class ValveProcess : KMonoBehaviour, ISaveLoadable
    {
        #region based on ONI code : ValveBase

        [SerializeField]
        public ConduitType conduitType;

        [MyCmpGet]
        protected KBatchedAnimController controller;

        private int curFlowIdx;
        private int inputCell;
        private int outputCell;

        [SerializeField]
        public ValveBase.AnimRangeInfo[] animFlowRanges;

        [Serialize]
        public float CurrentFlow { get; set; }

        public HandleVector<int>.Handle AccumulatorHandle { get; protected set; }

        [SerializeField]
        public float MaxFlow { get; set; }


        ConduitFlow _FlowMgr = null;
        ConduitFlow FlowMgr
        {
            get
            {
                if (_FlowMgr == null)
                    _FlowMgr = Conduit.GetFlowManager(this.conduitType);

                return _FlowMgr;
            }
            set
            {
                _FlowMgr = value;
            }
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            this.AccumulatorHandle = Game.Instance.accumulators.Add("Flow", (KMonoBehaviour)this);

            OnONIValvePrefabInit();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            
            Building component = GetComponent<Building>();

            this.inputCell = component.GetUtilityInputCell();
            this.outputCell = component.GetUtilityOutputCell();
            FlowMgr.AddConduitUpdater(OnConduitUpdate);
            this.UpdateAnim();
            this.OnCmpEnable();

            OnONIValveSpawn();
        }

        protected virtual void OnCleanUp2()
        {
            Game.Instance.accumulators.Remove(this.AccumulatorHandle);
            FlowMgr.RemoveConduitUpdater(OnConduitUpdate);

            base.OnCleanUp();
        }

        public virtual void UpdateAnim()
        {
            float averageRate = Game.Instance.accumulators.GetAverageRate(this.AccumulatorHandle);
            if ((double)averageRate > 0.0)
            {
                for (int index = 0; index < this.animFlowRanges.Length; ++index)
                {
                    if ((double)averageRate <= (double)this.animFlowRanges[index].minFlow)
                    {
                        if (this.curFlowIdx == index)
                            break;
                        this.curFlowIdx = index;
                        //this.controller.Play((HashedString)this.animFlowRanges[index].animName, (double)averageRate > 0.0 ? (KAnim.PlayMode)0 : (KAnim.PlayMode)1, 1f, 0.0f);
                        this.controller.Play((HashedString)this.animFlowRanges[index].animName, (double)averageRate > 0.0 ? KAnim.PlayMode.Loop : KAnim.PlayMode.Once, 1f, 0.0f);
                        break;
                    }
                }
            }
            else
                //this.controller.Play((HashedString)"off", (KAnim.PlayMode)1, 1f, 0.0f);
                this.controller.Play((HashedString)"off", KAnim.PlayMode.Once, 1f, 0.0f);

        }

        [Serializable]
        public struct AnimRangeInfo
        {
            public float minFlow;
            public string animName;

            public AnimRangeInfo(float min_flow, string anim_name)
            {
                this.minFlow = min_flow;
                this.animName = anim_name;
            }
        }

        void OnConduitUpdate(float dt)
        {
            ConduitFlow.Conduit conduit = FlowMgr.GetConduit(this.inputCell);
            if (!FlowMgr.HasConduit(this.inputCell) || !FlowMgr.HasConduit(this.outputCell))
            {
                this.UpdateAnim();
            }
            else
            {
                ConduitFlow.ConduitContents contents = conduit.GetContents(FlowMgr);
                float mass = Mathf.Min(contents.mass, this.CurrentFlow * dt);
                if ((double)mass > 0.0)
                {
                    int disease_count = (int)((double)(mass / contents.mass) * (double)contents.diseaseCount);
                    float num = FlowMgr.AddElement(this.outputCell, contents.element, mass, contents.temperature, contents.diseaseIdx, disease_count);
                    Game.Instance.accumulators.Accumulate(this.AccumulatorHandle, num);
                    if ((double)num > 0.0)
                    {
                        FlowMgr.RemoveElement(this.inputCell, num);

                        this._DesiredFlow -= num;
                        
                        this.ChangeFlow(this.DesiredFlow);

                        this.CurrentFlow = this.DesiredFlow;
                    }
                }
                this.UpdateAnim();
            }
        }

        #endregion

        #region based on ONI code : Valve

        [Serialize]
        float _DesiredFlow = 0.5f;

        public float QueuedMaxFlow
        {
            get
            {
                return -1f;
            }
        }

        public float DesiredFlow
        {
            get
            {
                return this._DesiredFlow;
            }
        }

        //public float MaxFlow
        //{
        //    get
        //    {
        //        return this.valveBase.MaxFlow;
        //    }
        //}

        void OnONIValvePrefabInit()
        {
            //this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
            //this.synchronizeAnims = false;
            //this.valveBase.CurrentFlow = this.valveBase.MaxFlow;
            this.CurrentFlow = this.MaxFlow;
            this._DesiredFlow = this.MaxFlow;
        }

        void OnONIValveSpawn()
        {
            this.ChangeFlow(this._DesiredFlow);
        }

        public void ChangeFlow(float amount)
        {
            this._DesiredFlow = Mathf.Clamp(amount, 0.0f, this.MaxFlow);
            KSelectable component = GetComponent<KSelectable>();

            Database.BuildingStatusItems buildingStatusItems = Db.Get().BuildingStatusItems;

            component.ToggleStatusItem(buildingStatusItems.PumpingLiquidOrGas, this._DesiredFlow >= 0.0f, this.AccumulatorHandle);
            //if (DebugHandler.InstantBuildMode)
            //    this.UpdateFlow();
            //else if (this._DesiredFlow != this.CurrentFlow)
            //{
            //    if (this.chore != null)
            //        return;
            //    component.AddStatusItem(Db.Get().BuildingStatusItems.ValveRequest, (object)this);
            //    component.AddStatusItem(Db.Get().BuildingStatusItems.PendingWork, (object)this);
            //    this.chore = (Chore)new WorkChore<Valve>(Db.Get().ChoreTypes.Toggle, (IStateMachineTarget)this, (ChoreProvider)null, true, (System.Action<Chore>)null, (System.Action<Chore>)null, (System.Action<Chore>)null, true, (ScheduleBlockType)null, false, false, (KAnimFile)null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
            //}
            //else
            //{
            //    if (this.chore != null)
            //    {
            //        this.chore.Cancel("desiredFlow == currentFlow");
            //        this.chore = (Chore)null;
            //    }
            //    component.RemoveStatusItem(Db.Get().BuildingStatusItems.ValveRequest, false);
            //    component.RemoveStatusItem(Db.Get().BuildingStatusItems.PendingWork, false);
            //}
        }

        public void UpdateFlow()
        {
            this.CurrentFlow = this._DesiredFlow;
            this.UpdateAnim();
        }

        #endregion

    }
}
