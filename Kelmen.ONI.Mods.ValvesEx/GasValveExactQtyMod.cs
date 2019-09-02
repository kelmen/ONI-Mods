using Harmony;

namespace Kelmen.ONI.Mods.ValvesEx
{
    public class GasValveExactQtyMod
    {
        [HarmonyPatch(typeof(GeneratedBuildings))]
        [HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
        public class GeneratedBuildings_LoadGeneratedBuildings
        {
            public static void Prefix()
            {
                GasValveExactQty.SetDescriptions();
                GasValveExactQty.SetMenu();
            }
        }

        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch(nameof(Db.Initialize))]
        public class Db_Initialize
        {
            public static void Prefix()
            {
                GasValveExactQty.SetTechTree();
            }
        }

        [HarmonyPatch(typeof(BuildingComplete))]
        [HarmonyPatch("OnSpawn")]
        public static class ChangeGasValveExactQtyColor
        {
            public static void Postfix(BuildingComplete __instance)
            {
                if (string.Compare(__instance.name, (GasValveExactQty.ID + "Complete")) == 0)
                {
                    var kanim = __instance.GetComponent<KAnimControllerBase>();
                    if (kanim == null) return;

                    kanim.TintColour = GasValveExactQty.ChangeColor();
                }
            }
        }

    }
}
