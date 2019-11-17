using KSerialization;
using TUNING;
using UnityEngine;

using Kelmen.ONI.Mods.Shared.CaiLib.Utils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.BuildingUtils;
using static Kelmen.ONI.Mods.Shared.CaiLib.Utils.StringUtils;

namespace Kelmen.ONI.Mods.Storages
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class HighInflowLiquidReservoir : LiquidReservoirConfig
    {
        public new const string ID = "Kelmen-HighInflowLiquidReservoir";

        public const string DisplayName = "High Inflow Liquid Tank";
        public const string Description = "Vanilla Liquid Reservoir with a second liquid input.";
        public const string Effect = "Liquid tank with 2 input connectors.";

        public const ConduitType FluidType = ConduitType.Liquid;

        //[SerializeField]
        public ConduitPortInfo InputPort2Info = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 1));
        //buildingDef.UtilityInputOffset = new CellOffset(1, 2);
        //buildingDef.UtilityOutputOffset = new CellOffset(0, 0);

        public override BuildingDef CreateBuildingDef()
        {
            var buildingDef = base.CreateBuildingDef();

            buildingDef.PrefabID = ID;

            buildingDef.PermittedRotations = PermittedRotations.FlipH;

            buildingDef.InitDef();

            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            base.ConfigureBuildingTemplate(go, prefab_tag);

            //var conduitConsumer = go.AddOrGet<HighInflowLiquidReservoirProcess>();
            //conduitConsumer.InputPort2Info = this.InputPort2Info;

            var conduit1stConsumer = go.GetComponent<ConduitConsumer>();

            var conduit2ndConsumer = go.AddComponent<ConduitConsumer>();
            conduit2ndConsumer.conduitType = ConduitType.Liquid;
            conduit2ndConsumer.ignoreMinMassCheck = true;
            conduit2ndConsumer.forceAlwaysSatisfied = true;
            conduit2ndConsumer.alwaysConsume = true;
            conduit2ndConsumer.capacityKG = conduit1stConsumer.capacityKG;
            conduit2ndConsumer.useSecondaryInput = true;
        }

        void AttachInputPort2(GameObject go)
        {
            go.AddComponent<ConduitSecondaryInput>().portInfo = this.InputPort2Info;
        }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
            base.DoPostConfigurePreview(def, go);
            this.AttachInputPort2(go);
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            base.DoPostConfigureUnderConstruction(go);
            this.AttachInputPort2(go);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            base.DoPostConfigureComplete(go);

            //var conduitConsumer = go.AddOrGet<HighInflowLiquidReservoirProcess>();
            //conduitConsumer.InputPort2Info = this.InputPort2Info;
        }

        public static void SetDescriptions()
        {
            AddBuildingStrings(ID, DisplayName, Description, Effect);
        }

        public static void SetMenu()
        {
            AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Plumbing, ID);
        }

        public static void SetTechTree()
        {
            AddBuildingToTechnology(GameStrings.Technology.Liquids.ImprovedPlumbing, ID);
        }

        public static Color32 ChangeColor()
        {
            return new Color32(255, 255, 128, 255);
        }

    }
}
