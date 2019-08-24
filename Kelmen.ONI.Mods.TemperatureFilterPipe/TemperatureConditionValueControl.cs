using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Kelmen.ONI.Mods.TemperatureFilterPipe
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class TemperatureConditionValueControl : ISliderControl
    {
        //[Serialize, SerializeField]
        public float TemperatureConditionValue = 273.15f; // 0 C

        List<float> DataList = new List<float>();

        #region ISliderControl

        public string SliderTitleKey => "temperature condition value";

        public string SliderUnits => "positive numerical";

        public float GetSliderMax(int index)
        {
            return 20000; // 19,726.85 degree Celsius
        }

        public float GetSliderMin(int index)
        {
            return 0; // -273.15 degree Celsius
        }

        public string GetSliderTooltip()
        {
            return "temperature condition value";
        }

        public string GetSliderTooltipKey(int index)
        {
            return $"TemperatureConditionValueControl.GetSliderTooltipKey({index})";
        }

        public float GetSliderValue(int index)
        {
            return DataList[index];
        }

        public void SetSliderValue(float percent, int index)
        {
            DataList[index] = percent;
        }

        public int SliderDecimalPlaces(int index)
        {
            return 2;
        }

        #endregion
    }
}
