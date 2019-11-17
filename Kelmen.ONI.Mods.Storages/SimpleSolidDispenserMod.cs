﻿using Harmony;

namespace Kelmen.ONI.Mods.Storages
{
    public class SimpleSolidDispenserMod
    {
        [HarmonyPatch(typeof(GeneratedBuildings))]
        [HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
        public class GeneratedBuildings_LoadGeneratedBuildings
        {
            public static void Prefix()
            {
                SimpleSolidDispenser.SetDescriptions();
                SimpleSolidDispenser.SetMenu();
            }
        }

        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch(nameof(Db.Initialize))]
        public class Db_Initialize
        {
            public static void Prefix()
            {
                SimpleSolidDispenser.SetTechTree();
            }
        }

        [HarmonyPatch(typeof(BuildingComplete))]
        [HarmonyPatch("OnSpawn")]
        public static class ChangeLiquidTemperatureFilterColor
        {
            public static void Postfix(BuildingComplete __instance)
            {
                if (string.Compare(__instance.name, (SimpleSolidDispenser.ID + "Complete")) == 0)
                {
                    var kanim = __instance.GetComponent<KAnimControllerBase>();
                    if (kanim == null) return;

                    kanim.TintColour = SimpleSolidDispenser.ChangeColor();
                }
            }
        }

    }
}
