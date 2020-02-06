using KSerialization;
using UnityEngine;
using System.Linq;

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

        protected float MassMoved = 0;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
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
            MassMoved = 0;
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
                float massSrc = Mathf.Min(contents.mass, this.DesiredFlow);
                if (massSrc > 0)
                {
                    int disease_count = (int)((massSrc / contents.mass) * contents.diseaseCount);
                    var massMoved = FlowMgr.AddElement(this.OutputCell, contents.element, massSrc, contents.temperature, contents.diseaseIdx, disease_count);
                    MassMoved += massMoved;
                    if (massMoved > 0)
                    {
                        FlowMgr.RemoveElement(this.InputCell, massMoved);

                        DesiredFlow -= massMoved;
                    }
                }
            }

            this.UpdateAnim();
        }

        [MyCmpGet]
        protected KBatchedAnimController controller;

        public virtual void UpdateAnim()
        {
            if (MassMoved > 0)
            {
                foreach(var anim in animFlowRanges.Reverse())
                {
                    if (MassMoved > anim.minFlow)
                    {
                        MassMoved = 0;
                        this.controller.Play(anim.animName, KAnim.PlayMode.Loop);
                        return;
                    }
                }
            }
            
            this.controller.Play("off", KAnim.PlayMode.Once);
        }

    }
}
