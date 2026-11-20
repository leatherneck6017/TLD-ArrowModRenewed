using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppTLD.Gear;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ArrowModRenewed.src
{
    internal static class Patches
    {
        [HarmonyPatch(typeof(Panel_Crafting), nameof(Panel_Crafting.Initialize))]
        public static class Panel_Crafting_Initialize
        {
            private static void Postfix()
            {
                BlueprintManager blueprintManager = InterfaceManager.GetInstance().m_BlueprintManager;
                if (blueprintManager == null || HasBlueprint(blueprintManager, "BLUEPRINT_GEAR_ArrowShaft_ArrowModRenewed_A"))
                {
                    return;
                }

                ArrowModRenewed.Log("Adding blueprints.");

                GearItem? shaftPrefab = GetGearItemPrefab("GEAR_ArrowShaft");
                GearItem? hardwoodPrefab = GetGearItemPrefab("GEAR_Hardwood");
                ToolsItem? knifePrefab = GetToolItemPrefab("GEAR_Knife");

                if (shaftPrefab == null || hardwoodPrefab == null)
                {
                    ArrowModRenewed.Log("Failed to load arrow shaft or hardwood prefabs. Aborting blueprint registration.");
                    return;
                }

                // Recipe Shaft A: Arrow Shafts from Hardwood
                blueprintManager.m_AllBlueprints.Add(CreateBlueprint(
                    name: "BLUEPRINT_GEAR_ArrowShaft_ArrowModRenewed_A",
                    resultGear: shaftPrefab,
                    resultCount: 6,
                    requiredTool: knifePrefab,
                    location: CraftingLocation.Workbench,
                    durationMinutes: 180,
                    ingredients: new[]
                    {
                        (hardwoodPrefab, 1)
                    }
                ));
                ArrowModRenewed.Log("Added: BLUEPRINT_GEAR_ArrowShaft_ArrowModRenewed_A");
            }

            private static bool HasBlueprint(BlueprintManager manager, string blueprintName)
            {
                foreach (BlueprintData bp in manager.m_AllBlueprints)
                {
                    if (bp != null && bp.name == blueprintName)
                    {
                        return true;
                    }
                }
                return false;
            }

            private static BlueprintData CreateBlueprint(
                string name,
                GearItem resultGear,
                int resultCount,
                ToolsItem? requiredTool,
                CraftingLocation location,
                int durationMinutes,
                (GearItem item, int count)[] ingredients)
            {
                var requiredGearArray = new Il2CppReferenceArray<BlueprintData.RequiredGearItem>(ingredients.Length);
                for (int i = 0; i < ingredients.Length; i++)
                {
                    requiredGearArray[i] = new BlueprintData.RequiredGearItem
                    {
                        m_Item = ingredients[i].item,
                        m_Count = ingredients[i].count
                    };
                }

                BlueprintData bp = ScriptableObject.CreateInstance<BlueprintData>();
                bp.name = name;

                // Inputs
                bp.m_RequiredTool = requiredTool;
                bp.m_OptionalTools = new Il2CppReferenceArray<ToolsItem>(0);
                bp.m_RequiredGear = requiredGearArray;
                bp.m_RequiredLiquid = new Il2CppReferenceArray<BlueprintData.RequiredLiquid>(0);
                bp.m_RequiredPowder = new Il2CppReferenceArray<BlueprintData.RequiredPowder>(0);

                // Outputs
                bp.m_CraftedResultGear = resultGear;
                bp.m_CraftedResultCount = resultCount;
                bp.m_CraftingResultType = CraftingResult.StandardGear;
                bp.m_CraftedResultDecoration = null;
                bp.m_CraftingIcon = new AssetReferenceTexture2D("");
                bp.m_XPModesToDisable = new();

                // Process
                bp.m_UsesPhoto = false;
                bp.m_Locked = true;
                bp.m_IgnoreLockInSurvival = false;
                bp.m_AppearsInStoryOnly = false;
                bp.m_AppearsInSurvivalOnly = false;
                bp.m_RequiresLight = true;
                bp.m_RequiresLitFire = false;
                bp.m_RequiredCraftingLocation = location;
                bp.m_DurationMinutes = durationMinutes;
                bp.m_CraftingAudio = MakeAudioEvent("PLAY_CRAFTINGARROWS");

                // Skills
                bp.m_AppliedSkill = SkillType.None;
                bp.m_ImprovedSkill = SkillType.None;
                bp.m_CanIncreaseRepairSkill = false;

                return bp;
            }
        }

        [HarmonyPatch(typeof(BlueprintDisplayItem), nameof(BlueprintDisplayItem.Setup))]
        public static class BlueprintDisplayItem_Setup
        {
            private static void Postfix(BlueprintDisplayItem __instance, BlueprintData bpi)
            {
                if (__instance == null || bpi == null || __instance.m_DisplayName == null)
                {
                    return;
                }

                if (bpi.name == "BLUEPRINT_GEAR_ArrowShaft_ArrowModRenewed_A")
                {
                    __instance.m_DisplayName.text = $"Arrow Shaft (Hardwood) ({bpi.m_CraftedResultCount})";
                }
            }
        }

        [HarmonyPatch(typeof(Panel_Crafting), nameof(Panel_Crafting.RefreshSelectedBlueprint))]
        public static class Panel_Crafting_RefreshSelectedBlueprint
        {
            private static void Postfix(Panel_Crafting __instance)
            {
                BlueprintData? bp = __instance?.SelectedBPI;
                if (bp == null)
                {
                    return;
                }

                if (bp.name == "BLUEPRINT_GEAR_ArrowShaft_ArrowModRenewed_A")
                {
                    if (__instance!.m_SelectedName != null)
                    {
                        __instance.m_SelectedName.text = "Arrow Shaft (Hardwood)";
                    }

                    if (__instance.m_SelectedDescription != null)
                    {
                        __instance.m_SelectedDescription.text = "A batch of arrow shafts carved from seasoned hardwood.";
                    }
                }
            }
        }

        [HarmonyPatch(typeof(BlueprintManager), nameof(BlueprintManager.GetAllBlueprints))]
        public static class BlueprintManager_GetAllBlueprints
        {
            public static void Postfix(BlueprintManager __instance)
            {
                if (__instance?.m_AllBlueprints == null)
                {
                    return;
                }

                bool noBench = Settings.options.arrowsNoBench == YesNo.No;
                CraftingLocation arrowBench = noBench ? CraftingLocation.Anywhere : CraftingLocation.Workbench;
                int noBenchPenalty = noBench ? Settings.options.arrowNoBenchTime : 0;
                int archeryLevel = GameManager.GetSkillArchery().GetCurrentTierNumber() + 1;

                foreach (BlueprintData val in __instance.m_AllBlueprints)
                {
                    if (val == null)
                    {
                        continue;
                    }

                    if (val.name == "BLUEPRINT_GEAR_ArrowShaft_ArrowModRenewed_A")
                    {
                        val.Locked = !(Settings.options.craftArrowFromWood == YesNo.Yes
                            && archeryLevel >= Settings.options.craftArrowFromWoodLevel);
                    }
                    else if (val.m_CraftedResultGear != null && val.m_CraftedResultGear.name == "GEAR_ArrowHead")
                    {
                        val.m_DurationMinutes = Settings.options.arrowHeadCraftTime * Mathf.FloorToInt(val.m_CraftedResultCount / 2f);
                    }
                    else if (val.m_CraftedResultGear != null && val.m_CraftedResultGear.name == "GEAR_Arrow")
                    {
                        val.m_RequiredCraftingLocation = arrowBench;
                        val.m_DurationMinutes = (Settings.options.arrowCraftTime + noBenchPenalty) * val.m_CraftedResultCount;
                    }
                }
            }
        }

        public static GearItem? GetGearItemPrefab(string name)
        {
            try
            {
                GameObject prefab = Addressables.LoadAssetAsync<GameObject>(name).WaitForCompletion();
                return prefab != null ? prefab.GetComponent<GearItem>() : null;
            }
            catch
            {
                return null;
            }
        }

        public static ToolsItem? GetToolItemPrefab(string name)
        {
            try
            {
                GameObject prefab = Addressables.LoadAssetAsync<GameObject>(name).WaitForCompletion();
                return prefab != null ? prefab.GetComponent<ToolsItem>() : null;
            }
            catch
            {
                return null;
            }
        }

        // from CraftingRevisions by ds5678 and STBlade
        public static Il2CppAK.Wwise.Event? MakeAudioEvent(string? eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                return null;
            }

            uint eventId = AkSoundEngine.GetIDFromString(eventName);
            if (eventId <= 0 || eventId >= 4294967295)
            {
                return null;
            }

            Il2CppAK.Wwise.Event newEvent = new()
            {
                WwiseObjectReference = new WwiseEventReference
                {
                    objectName = eventName,
                    id = eventId
                }
            };
            return newEvent;
        }
    }
}
