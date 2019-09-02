using KSerialization;

namespace Kelmen.ONI.Mods.ValvesEx
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class LiquidValveProcess : ValveProcessBase
    {
        public override ConduitType ConduitType => ConduitType.Liquid;

        public override ValveBase.AnimRangeInfo[] animFlowRanges => 
            new ValveBase.AnimRangeInfo[3]
            {
              new ValveBase.AnimRangeInfo(3f, "lo"),
              new ValveBase.AnimRangeInfo(7f, "med"),
              new ValveBase.AnimRangeInfo(10f, "hi")
            };

    }
}
