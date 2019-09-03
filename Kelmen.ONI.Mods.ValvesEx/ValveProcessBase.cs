using KSerialization;
using UnityEngine;

namespace Kelmen.ONI.Mods.ValvesEx
{
    public abstract partial class ValveProcessBase : KMonoBehaviour
    {
        public abstract ConduitType ConduitType { get; }

        public abstract ValveBase.AnimRangeInfo[] animFlowRanges { get; }

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

        public float MaxFlow
        {
            get
            {
                switch (this.ConduitType)
                {
                    case ConduitType.Liquid:
                        return 10;

                    case ConduitType.Gas:
                        return 1;
                }

                return 0;
            }
        }

        public int InputCell { get; protected set; }
        public int OutputCell { get; protected set; }

        /// <summary>
        /// KG
        /// </summary>
        [SerializeField]
        [Serialize]
        public float DesiredFlow { get; set; }

        //[MyCmpReq]
        //ValveData ValveData = null;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            this.FlowAccumulator = Game.Instance.accumulators.Add("Flow", (KMonoBehaviour)this);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            Building component = GetComponent<Building>();
            InputCell = component.GetUtilityInputCell();
            OutputCell = component.GetUtilityOutputCell();

            FlowMgr.AddConduitUpdater(OnConduitUpdate);

            DataOnSpawn();

            this.UpdateAnim();
            this.OnCmpEnable();
        }

        protected override void OnCleanUp()
        {
            FlowMgr.RemoveConduitUpdater(OnConduitUpdate);

            Game.Instance.accumulators.Remove(this.FlowAccumulator);

            base.OnCleanUp();
        }

        protected virtual void OnConduitUpdate(float data)
        {
            if (!FlowMgr.HasConduit(this.InputCell) || !FlowMgr.HasConduit(this.OutputCell))
            {
            }
            else
            {
                ConduitFlow.Conduit conduit = FlowMgr.GetConduit(this.InputCell);
                ConduitFlow.ConduitContents contents = conduit.GetContents(FlowMgr);
                float massMove = Mathf.Min(contents.mass, this.DesiredFlow);
                if (massMove > 0f)
                {
                    int disease_count = (int)((massMove / contents.mass) * contents.diseaseCount);
                    FlowMgr.AddElement(this.OutputCell, contents.element, massMove, contents.temperature, contents.diseaseIdx, disease_count);
                    FlowMgr.RemoveElement(this.InputCell, massMove);

                    Game.Instance.accumulators.Accumulate(this.FlowAccumulator, massMove);

                    DesiredFlow -= massMove;
                }
            }

            this.UpdateAnim();
        }

        protected HandleVector<int>.Handle FlowAccumulator;
        int curFlowIdx;

        [MyCmpGet]
        protected KBatchedAnimController controller;

        public virtual void UpdateAnim()
        {
            float averageRate = Game.Instance.accumulators.GetAverageRate(this.FlowAccumulator);
            if ((double)averageRate > 0.0)
            {
                for (int index = 0; index < this.animFlowRanges.Length; ++index)
                {
                    if ((double)averageRate <= (double)this.animFlowRanges[index].minFlow)
                    {
                        if (this.curFlowIdx == index)
                            break;
                        this.curFlowIdx = index;
                        //this.controller.Play(this.animFlowRanges[index].animName, (double)averageRate > 0.0 ? (KAnim.PlayMode)0 : (KAnim.PlayMode)1, 1f, 0.0f);
                        this.controller.Play(this.animFlowRanges[index].animName, averageRate > 0 ? KAnim.PlayMode.Loop : KAnim.PlayMode.Once);
                        break;
                    }
                }
            }
            else
                this.controller.Play("off", KAnim.PlayMode.Once);
        }

    }
}
