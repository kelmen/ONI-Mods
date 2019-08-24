using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kelmen.ONI.Mods.TemperatureFilterPipe
{
    public class TemperatureFilterCfg : ISaveLoadable, IThresholdSwitch
    {
        #region IThresholdSwitch

        public float Threshold { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ActivateAboveThreshold { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public float CurrentValue => throw new NotImplementedException();

        public float RangeMin => throw new NotImplementedException();

        public float RangeMax => throw new NotImplementedException();

        public LocString Title => throw new NotImplementedException();

        public LocString ThresholdValueName => throw new NotImplementedException();

        public string AboveToolTip => throw new NotImplementedException();

        public string BelowToolTip => throw new NotImplementedException();

        public ThresholdScreenLayoutType LayoutType => throw new NotImplementedException();

        public int IncrementScale => throw new NotImplementedException();

        public NonLinearSlider.Range[] GetRanges => throw new NotImplementedException();

        public string Format(float value, bool units)
        {
            throw new NotImplementedException();
        }

        public float GetRangeMaxInputField()
        {
            throw new NotImplementedException();
        }

        public float GetRangeMinInputField()
        {
            throw new NotImplementedException();
        }

        public float ProcessedInputValue(float input)
        {
            throw new NotImplementedException();
        }

        public float ProcessedSliderValue(float input)
        {
            throw new NotImplementedException();
        }

        public LocString ThresholdValueUnits()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
