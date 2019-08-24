using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Kelmen.ONI.Mods.TemperatureFilterPipe
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class TemperatureConditionOperatorControl : ISliderControl
    {
        public enum ConditionOperators
        {
            //NoCondition = 0,
            LessThan = 1,
            LessThanOrEqual = 2,
            Equal = 3,
            EqualOrGreaterThan = 4,
            GreaterThan = 5,
        }

        //[Serialize, SerializeField]
        public ConditionOperators TemperatureConditionOperator = ConditionOperators.Equal;

        List<ConditionOperators> DataList = new List<ConditionOperators>();

        #region ISliderControl

        public string SliderTitleKey => "Condition Operator";

        public string SliderUnits => "kelvin";

        public float GetSliderMax(int index)
        {
            return 5; // ConditionOperators.GreaterThan
        }

        public float GetSliderMin(int index)
        {
            return 1; // ConditionOperators.LessThan
        }

        public string GetSliderTooltip()
        {
            return "temperatur condition operator";
        }

        public string GetSliderTooltipKey(int index)
        {
            return $"TemperatureConditionOperatorControl.GetSliderTooltipKey({index})";
        }

        public float GetSliderValue(int index)
        {
            var ret = DataList[index];
            return  (float)ret;
        }

        public void SetSliderValue(float percent, int index)
        {
            if (percent < 0)
                throw new Exception($"TemperatureConditionOperatorControl.SetSliderValue({percent}, {index})");

            int val = (int)Math.Abs(percent);
            DataList[index] = (ConditionOperators)val;
        }

        public int SliderDecimalPlaces(int index)
        {
            return 0;
        }

        #endregion
    }
}
