using KSerialization;
using STRINGS;
using UnityEngine;


namespace Kelmen.ONI.Mods.ConduitFilters.TemperatureFilters
{
    //[SerializationConfig(MemberSerialization.OptIn)]
    //public class TemperatureFilterData : KMonoBehaviour, ISaveLoadable, IThresholdSwitch
    public partial class TemperatureFilterProcess : IThresholdSwitch
    {
        //public ConduitPortInfo OutputPort2Info = null;
        //public int InputCell = -1;

        #region IThresholdSwitch

        [SerializeField]
        [Serialize]
        public float Threshold { get; set; }

        [SerializeField]
        [Serialize]
        public bool ActivateAboveThreshold { get; set; } = true;

        public float CurrentValue { get { return Conduit.GetFlowManager(this.OutputPort2Info.conduitType).GetContents(this.InputCell).temperature; } }

        public float RangeMin { get; set; } = 0;

        public float RangeMax { get; set; } = 9999;

        public LocString Title { get { return UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.TITLE; } }

        public LocString ThresholdValueName { get { return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE; } }

        public string AboveToolTip { get { return (string)UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_ABOVE; } }

        public string BelowToolTip { get { return (string)UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_BELOW; } }

        public ThresholdScreenLayoutType LayoutType { get { return ThresholdScreenLayoutType.SliderBar; } }

        public int IncrementScale { get { return 1; } }

        public NonLinearSlider.Range[] GetRanges
        {
            get
            {
                return new NonLinearSlider.Range[4]
                {
                    new NonLinearSlider.Range(25f, 260f),
                    new NonLinearSlider.Range(50f, 400f),
                    new NonLinearSlider.Range(12f, 1500f),
                    new NonLinearSlider.Range(13f, 10000f)
                };
            }
        }

        public string Format(float value, bool units)
        {
            return GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, units, false);
        }

        public float GetRangeMaxInputField()
        {
            return GameUtil.GetConvertedTemperature(this.RangeMax, false);
        }

        public float GetRangeMinInputField()
        {
            return GameUtil.GetConvertedTemperature(this.RangeMin, false);
        }

        public float ProcessedInputValue(float input)
        {
            return GameUtil.GetTemperatureConvertedToKelvin(input);
        }

        public float ProcessedSliderValue(float input)
        {
            return Mathf.Round(input);
        }

        public LocString ThresholdValueUnits()
        {
            switch (GameUtil.temperatureUnit)
            {
                case GameUtil.TemperatureUnit.Celsius:
                    return UI.UNITSUFFIXES.TEMPERATURE.CELSIUS;

                case GameUtil.TemperatureUnit.Fahrenheit:
                    return UI.UNITSUFFIXES.TEMPERATURE.FAHRENHEIT;

                case GameUtil.TemperatureUnit.Kelvin:
                    return UI.UNITSUFFIXES.TEMPERATURE.KELVIN;

            }

            return null;
        }

        #endregion

        public int GetOutputRouteIdx(float temperature, int outputRoute1Idx, int outputRoute2Idx)
        {
            if (this.ActivateAboveThreshold)
                return (temperature < this.Threshold) ? outputRoute1Idx : outputRoute2Idx;
            else
                return (temperature > this.Threshold) ? outputRoute1Idx : outputRoute2Idx;
        }
    }
}
