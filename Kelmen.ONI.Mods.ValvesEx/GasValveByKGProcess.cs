using KSerialization;

namespace Kelmen.ONI.Mods.ValvesEx
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class GasValveProcessByKG : GasValveProcessByG
    {
        public override GameUtil.MetricMassFormat Metric => GameUtil.MetricMassFormat.Kilogram;
    }
}
