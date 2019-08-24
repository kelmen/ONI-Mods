using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using NightLib;

namespace Kelmen.ONI.Mods.TemperatureFilterPipe
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class CfgData : KMonoBehaviour, ISaveLoadable
    {
        [Serialize, SerializeField]
        public TemperatureConditionOperatorControl.ConditionOperators TemperatureConditionOperator = TemperatureConditionOperatorControl.ConditionOperators.Equal;

        [Serialize, SerializeField]
        public float TemperatureConditionValue = 273.15f; // 0 C

        //protected override void OnSpawn()
        //{
        //    base.OnSpawn();

        //    //this.Subscribe(GameHashes.RefreshUserMenu, new Action<object>(this.OnRefreshUserMenu));
        //    //this.Subscribe(GameHashes.StatusChange, new Action<object>(this.OnRefreshUserMenu));
        //}

        //void OnRefreshUserMenu(object data)
        //{

        //}
    }
}
