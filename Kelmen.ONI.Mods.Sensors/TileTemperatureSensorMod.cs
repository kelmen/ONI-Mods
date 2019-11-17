using Harmony;

namespace Kelmen.ONI.Mods.Sensors
{
    public class TileTemperatureSensorMod
    {
        [HarmonyPatch(typeof(GeneratedBuildings))]
        [HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
        public class GeneratedBuildings_LoadGeneratedBuildings
        {
            public static void Prefix()
            {
                TileTemperatureSensor.SetDescriptions();
                TileTemperatureSensor.SetMenu();
            }
        }

        [HarmonyPatch(typeof(Db))]
        //[HarmonyPatch("Initialize")]
        [HarmonyPatch(nameof(Db.Initialize))]
        public class Db_Initialize
        {
            public static void Prefix()
            {
                TileTemperatureSensor.SetTechTree();
            }
        }

        //[HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
        [HarmonyPatch(typeof(BuildingComplete))]
        //[HarmonyPatch(nameof(BuildingComplete.OnSpawn))]
        [HarmonyPatch("OnSpawn")]
        public static class ChangeGasTemperatureFilterColor
        {
            //[HarmonyPostfix]
            public static void Postfix(BuildingComplete __instance)
            {
                if (string.Compare(__instance.name, (TileTemperatureSensor.ID + "Complete")) == 0)
                {
                    var kanim = __instance.GetComponent<KAnimControllerBase>();
                    if (kanim == null) return;

                    kanim.TintColour = TileTemperatureSensor.ChangeColor();
                }
            }
        }

    }
}
