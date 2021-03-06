﻿using Harmony;

namespace Kelmen.ONI.Mods.Doors
{
    public class SimplePressureDoorMod
    {
        [HarmonyPatch(typeof(GeneratedBuildings))]
        [HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
        public class GeneratedBuildings_LoadGeneratedBuildings
        {
            public static void Prefix()
            {
                SimplePressureDoor.SetDescriptions();
                SimplePressureDoor.SetMenu();
            }
        }

        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch(nameof(Db.Initialize))]
        public class Db_Initialize
        {
            public static void Prefix()
            {
                SimplePressureDoor.SetTechTree();
            }
        }

        [HarmonyPatch(typeof(BuildingComplete))]
        [HarmonyPatch("OnSpawn")]
        public static class ChangeLiquidTemperatureFilterColor
        {
            public static void Postfix(BuildingComplete __instance)
            {
                if (string.Compare(__instance.name, (SimplePressureDoor.ID + "Complete")) == 0)
                {
                    var kanim = __instance.GetComponent<KAnimControllerBase>();
                    if (kanim == null) return;

                    kanim.TintColour = SimplePressureDoor.ChangeColor();
                }
            }
        }
    }
}
