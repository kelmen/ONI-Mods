using KSerialization;

namespace Kelmen.ONI.Mods.ValvesEx
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class GasValveProcess : ValveProcessBase
    {
        public override ConduitType ConduitType => ConduitType.Gas;

        public override ValveBase.AnimRangeInfo[] animFlowRanges =>
            new ValveBase.AnimRangeInfo[3]
            {
              new ValveBase.AnimRangeInfo(0.25f, "lo"),
              new ValveBase.AnimRangeInfo(0.5f, "med"),
              new ValveBase.AnimRangeInfo(0.75f, "hi")
            };

    }
}
