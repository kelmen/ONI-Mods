using KSerialization;
using System;
using UnityEngine;

namespace Kelmen.ONI.Mods.Sensors
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public partial class TileTemperatureSensorProcess : LogicTemperatureSensor
    {
        public float Temperature { get; set; } = 0;

        public new float GetTemperature()
        {
            return this.Temperature;
        }

        int? _CellIdx = null;
        int CellIdx
        {
            get
            {
                if (_CellIdx == null) _CellIdx = Grid.PosToCell(this);

                return _CellIdx.Value;
            }
        }

        #region ISim200ms

        public new void Sim200ms(float dt)
        {
            int cell = CellIdx;

            //if ((double)Grid.Mass[cell] <= 0.0)
            //    return;

            this.Temperature = Grid.Temperature[cell];

            if (this.activateOnWarmerThan)
            {
                if ((this.Temperature <= this.thresholdTemperature || this.IsSwitchedOn) && (this.Temperature >= this.thresholdTemperature || !this.IsSwitchedOn))
                    return;

                this.Toggle();
            }
            else
            {
                if ((this.Temperature <= this.thresholdTemperature || !this.IsSwitchedOn) && (this.Temperature >= this.thresholdTemperature || this.IsSwitchedOn))
                    return;

                this.Toggle();
            }
        }

        #endregion
    }
}
