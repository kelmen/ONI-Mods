using Harmony;

namespace Kelmen.ONI.Mods.Power.Transformers
{
    public class MediumPowerTransformerMod
    {
        [HarmonyPatch(typeof(GeneratedBuildings))]
        [HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
        public class GeneratedBuildings_LoadGeneratedBuildings
        {
            public static void Prefix()
            {
                MediumPowerTransformer.SetDescriptions();
                MediumPowerTransformer.SetMenu();
            }
        }

        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch(nameof(Db.Initialize))]
        public class Db_Initialize
        {
            public static void Prefix()
            {
                MediumPowerTransformer.SetTechTree();
            }
        }

        [HarmonyPatch(typeof(BuildingComplete))]
        [HarmonyPatch("OnSpawn")]
        public static class ChangeMediumPowerTransformerColor
        {
            public static void Postfix(BuildingComplete __instance)
            {
                if (string.Compare(__instance.name, (MediumPowerTransformer.ID + "Complete")) == 0)
                {
                    var kanim = __instance.GetComponent<KAnimControllerBase>();
                    if (kanim == null) return;

                    kanim.TintColour = MediumPowerTransformer.ChangeColor();
                }
            }
        }
    }
}
