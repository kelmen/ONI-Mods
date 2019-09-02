using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Kelmen.ONI.Mods.ValvesEx
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class ValveProcess : ValveBase, ISaveLoadable
    {
        ConduitFlow _FlowMgr = null;
        ConduitFlow FlowMgr
        {
            get
            {
                if (_FlowMgr == null)
                    _FlowMgr = Conduit.GetFlowManager(this.conduitType);

                return _FlowMgr;
            }
        }

        public int InputCell { get; protected set; }
        public int OutputCell { get; protected set; }

        [MyCmpReq]
        Valve ValveData = null;

        protected override void OnSpawn()
        {
            base.OnSpawn();

            Building component = GetComponent<Building>();
            InputCell = component.GetUtilityInputCell();
            OutputCell = component.GetUtilityOutputCell();

            FlowMgr.AddConduitUpdater(OnConduitUpdate);
        }

        protected override void OnCleanUp()
        {
            FlowMgr.RemoveConduitUpdater(OnConduitUpdate);

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
                float mass = Mathf.Min(contents.mass, this.CurrentFlow * data);
                if ((double)mass > 0.0)
                {
                    //int disease_count = (int)((mass / contents.mass) * contents.diseaseCount);
                    //float num = flowManager.AddElement(this.outputCell, contents.element, mass, contents.temperature, contents.diseaseIdx, disease_count);
                    float num = mass;
                    if ((double)num > 0.0)
                    {
                        var remain = ValveData.DesiredFlow - num;
                        ValveData.ChangeFlow(remain);
                    }
                }

            }
        }
    }
}
