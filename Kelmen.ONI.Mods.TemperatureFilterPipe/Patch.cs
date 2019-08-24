using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Kelmen.ONI.Mods.TemperatureFilterPipe
{
    public class Patch
    {
        [HarmonyPatch(typeof(GeneratedBuildings))]
        [HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
        public class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                GasTemperatureFilterCfg.SetDescriptions();
                GasTemperatureFilterCfg.SetMenu();
            }
        }

        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch("Initialize")]
        public class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                GasTemperatureFilterCfg.SetTechTree();
            }
        }
    }
}
