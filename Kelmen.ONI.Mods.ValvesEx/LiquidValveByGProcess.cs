using KSerialization;

namespace Kelmen.ONI.Mods.ValvesEx
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class LiquidValveByGProcess : ValveProcessBase
    {
        public override ConduitType ConduitType => ConduitType.Liquid;

        public override GameUtil.MetricMassFormat Metric => GameUtil.MetricMassFormat.Gram;

        public override ValveBase.AnimRangeInfo[] animFlowRanges =>
            new ValveBase.AnimRangeInfo[3]
            {
              new ValveBase.AnimRangeInfo(2.5f, "lo"),
              new ValveBase.AnimRangeInfo(5f, "med"),
              new ValveBase.AnimRangeInfo(7.5f, "hi")
            };

    }
}
