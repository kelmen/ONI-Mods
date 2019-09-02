using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Kelmen.ONI.Mods.ValvesEx
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public partial class ValveProcess : KMonoBehaviour, ISaveLoadable
    {
        [SerializeField]
        [Serialize]
        public ConduitType ConduitType { get; set; }

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
                switch(this.ConduitType)
                {
                    case ConduitType.Liquid:return 10;
                    case ConduitType.Gas: return 1;
                }

                return 0;
            }
        }

        public int InputCell { get; protected set; }
        public int OutputCell { get; protected set; }

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
                    float num = FlowMgr.AddElement(this.OutputCell, contents.element, massMove, contents.temperature, contents.diseaseIdx, disease_count);

                    Game.Instance.accumulators.Accumulate(this.FlowAccumulator, num);

                    if ((double)num > 0.0)
                        FlowMgr.RemoveElement(this.InputCell, num);

                    float remain = DesiredFlow - massMove;
                    ChangeFlowByKG(remain);
                }

            }

            this.UpdateAnim();
        }

        public void ChangeFlowByKG(float kiloGram)
        {
            if (kiloGram > MaxFlow) return;

            this.DesiredFlow = kiloGram;
        }

        public void ChangeFlowByG(float gram)
        {
            ChangeFlowByKG(gram * 1000f);
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

        [SerializeField]
        public ValveBase.AnimRangeInfo[] animFlowRanges;


    }
}
