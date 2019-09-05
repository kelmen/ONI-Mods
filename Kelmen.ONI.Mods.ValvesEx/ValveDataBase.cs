using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kelmen.ONI.Mods.ValvesEx
{
    public abstract partial class ValveProcessBase : ISingleSliderControl
    {
        public abstract GameUtil.MetricMassFormat Metric { get; }

        float ValeDataValue
        {
            get { return this.DesiredFlow * MetricConvertion; }
            set { this.DesiredFlow = value / MetricConvertion; }
        }

        float? _MetricConvertion = null;
        float MetricConvertion
        {
            get
            {
                if (_MetricConvertion == null)
                {
                    switch (Metric)
                    {
                        case GameUtil.MetricMassFormat.Kilogram:
                            _MetricConvertion = 1;
                            break;

                        case GameUtil.MetricMassFormat.Gram:
                            _MetricConvertion = 1000;
                            break;

                        default:
                            _MetricConvertion = 0;
                            break;
                    }
                }
                return _MetricConvertion.Value;
            }
        }

        void DataOnSpawn()
        {
        }

        #region ISingleSliderControl

        public string SliderTitleKey => //string.Format("STRINGS.UI.UISIDESCREENS.{0}.TITLE", GetSliderTooltipKey(0));
            "Eaxct Quantity Valve";

        public string SliderUnits => " " + Metric.ToString();

        public float GetSliderMax(int index)
        {
            return 1000;
        }

        public float GetSliderMin(int index)
        {
            return 0;
        }

        public string GetSliderTooltip()
        {
            return "Adjust the exact quantity of materials to be flow through.";
        }

        public string GetSliderTooltipKey(int index)
        {
            return "Kelmen-ExactValveDataSliderTooltipKey";
        }

        public float GetSliderValue(int index)
        {
            return ValeDataValue;
        }

        public void SetSliderValue(float percent, int index)
        {
            ValeDataValue = percent;
        }

        public int SliderDecimalPlaces(int index)
        {
            return 3;
        }

        #endregion

    }
}
