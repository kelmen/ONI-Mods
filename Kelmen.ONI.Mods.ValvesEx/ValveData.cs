using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Kelmen.ONI.Mods.ValvesEx
{
    public class ValveData : SideScreenContent
    {
        private ValveProcess targetValve;

        [Header("Slider")]
        [SerializeField]
        KSlider flowSlider = null;

        [SerializeField]
        LocText minFlowLabel = null;

        [SerializeField]
        LocText maxFlowLabel = null;

        [Header("Input Field")]
        [SerializeField]
        KNumberInputField numberInput = null;

        [SerializeField]
        LocText unitsLabel = null;

        bool isEditing = false;
        float targetFlow = 0f;

        #region SideScreenContent

        public override bool IsValidForTarget(GameObject target)
        {
            return (target.GetComponent<ValveProcess>() != null);
        }

        #endregion

        protected override void OnSpawn()
        {
            base.OnSpawn();
            unitsLabel.text = GameUtil.AddTimeSliceText((string)STRINGS.UI.UNITSUFFIXES.MASS.GRAM, GameUtil.TimeSlice.PerSecond);
            flowSlider.onReleaseHandle += OnReleaseHandle;
            this.flowSlider.onDrag += () => ReceiveValueFromSlider(this.flowSlider.value);
            this.flowSlider.onPointerDown += () => this.ReceiveValueFromSlider(this.flowSlider.value);
            this.flowSlider.onMove += () =>
            {
                this.ReceiveValueFromSlider(this.flowSlider.value);
                this.OnReleaseHandle();
            };
            this.numberInput.onEndEdit += () => this.ReceiveValueFromInput(this.numberInput.currentValue);
            this.numberInput.decimalPlaces = 1;
        }

        public void OnReleaseHandle()
        {
            this.targetValve.ChangeFlow(this.targetFlow);
        }

        void ReceiveValueFromSlider(float newValue)
        {
            newValue = Mathf.Round(newValue * 1000f) / 1000f;
            this.UpdateFlowValue(newValue);
        }

        void ReceiveValueFromInput(float input)
        {
            this.UpdateFlowValue(input / 1000f);
            this.targetValve.ChangeFlow(this.targetFlow);
        }

        void UpdateFlowValue(float newValue)
        {
            this.targetFlow = newValue;
            this.flowSlider.value = newValue;
            this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(newValue, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, false, "{0:0.#####}"));
        }

        public override void SetTarget(GameObject target)
        {
            this.targetValve = target.GetComponent<ValveProcess>();
            if (this.targetValve == null)
            {
                Debug.LogError((object)"The target object does not have a ValveProcess component.");
            }
            else
            {
                this.flowSlider.minValue = 0.0f;
                this.flowSlider.maxValue = this.targetValve.MaxFlow;
                this.flowSlider.value = this.targetValve.DesiredFlow;
                this.minFlowLabel.text = GameUtil.GetFormattedMass(0.0f, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, true, "{0:0.#}");
                this.maxFlowLabel.text = GameUtil.GetFormattedMass(this.targetValve.MaxFlow, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, true, "{0:0.#}");
                this.numberInput.minValue = 0.0f;
                this.numberInput.maxValue = this.targetValve.MaxFlow * 1000f;
                this.numberInput.displayName = GameUtil.GetFormattedMass(Mathf.Max(0.0f, this.targetValve.DesiredFlow), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, false, "{0:0.#####}");
                this.numberInput.Activate();
            }
        }

        public override void OnKeyDown(KButtonEvent e)
        {
            Debug.Log((object)"ValveData OnKeyDown");
            if (this.isEditing)
                e.Consumed = true;
            else
                base.OnKeyDown(e);
        }

        //[DebuggerHidden]
        //IEnumerator SettingDelay(float delay)
        //{
        //    // ISSUE: object of a compiler-generated type is created
        //    return (IEnumerator)new ValveData().SettingDelay(delay)
        //    {
        //        delay = delay,
        //        this = this
        //    };
        //}

    }
}
