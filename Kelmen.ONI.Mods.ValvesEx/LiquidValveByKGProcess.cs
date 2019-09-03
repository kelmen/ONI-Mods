using KSerialization;

namespace Kelmen.ONI.Mods.ValvesEx
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class LiquidValveByKGProcess : LiquidValveByGProcess
    {
        public override GameUtil.MetricMassFormat Metric => GameUtil.MetricMassFormat.Kilogram;
    }
}
