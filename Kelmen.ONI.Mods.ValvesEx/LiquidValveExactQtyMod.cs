using Harmony;

namespace Kelmen.ONI.Mods.ValvesEx
{
    public class LiquidValveExactQtyMod
    {
        [HarmonyPatch(typeof(GeneratedBuildings))]
        [HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
        public class GeneratedBuildings_LoadGeneratedBuildings
        {
            public static void Prefix()
            {
                LiquidValveExactQtyByG.SetDescriptions();
                LiquidValveExactQtyByG.SetMenu();

                LiquidValveExactQtyByKG.SetDescriptions();
                LiquidValveExactQtyByKG.SetMenu();
            }
        }

        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch(nameof(Db.Initialize))]
        public class Db_Initialize
        {
            public static void Prefix()
            {
                LiquidValveExactQtyByG.SetTechTree();

                LiquidValveExactQtyByKG.SetTechTree();
            }
        }

        [HarmonyPatch(typeof(BuildingComplete))]
        [HarmonyPatch("OnSpawn")]
        public static class ChangeLiquidValveExactQtyColor
        {
            public static void Postfix(BuildingComplete __instance)
            {
                if (string.Compare(__instance.name, (LiquidValveExactQtyByG.ID + "Complete")) == 0)
                {
                    var kanim = __instance.GetComponent<KAnimControllerBase>();
                    if (kanim == null) return;

                    kanim.TintColour = LiquidValveExactQtyByG.ChangeColor();
                }

                if (string.Compare(__instance.name, (LiquidValveExactQtyByKG.ID + "Complete")) == 0)
                {
                    var kanim = __instance.GetComponent<KAnimControllerBase>();
                    if (kanim == null) return;

                    kanim.TintColour = LiquidValveExactQtyByKG.ChangeColor();
                }
            }
        }
    }
}
