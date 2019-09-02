using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kelmen.ONI.Mods.ValvesEx
{
    public abstract partial class ValveProcessBase : ISingleSliderControl
    {
        float ValeDataValue
        {
            get { return this.DesiredFlow; }
            set { this.DesiredFlow = value; }
        }

        void DataOnSpawn()
        {
        }

        #region ISingleSliderControl

        public string SliderTitleKey => string.Format("STRINGS.UI.UISIDESCREENS.{0}.TITLE", GetSliderTooltipKey(0));

        public string SliderUnits => GameUtil.MetricMassFormat.Kilogram.ToString();

        public float GetSliderMax(int index)
        {
            return this.MaxFlow;
        }

        public float GetSliderMin(int index)
        {
            return 0f;
        }

        public string GetSliderTooltip()
        {
            return "Adjust the exact quantity of materials to be flow through.";
        }

        public string GetSliderTooltipKey(int index)
        {
            return "ExactValveDataSliderTooltipKey";
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
