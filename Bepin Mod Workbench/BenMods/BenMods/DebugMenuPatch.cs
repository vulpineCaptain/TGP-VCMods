using HarmonyLib;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.UI;
using BenMods.Debugging;
using BenMods.Combat;

namespace BenMods
{
    [HarmonyPatch(typeof(DebugMenu), "Update")]
    public static class DebugMenuPatch
    {
        private static readonly ManualLogSource Log =
            BepInEx.Logging.Logger.CreateLogSource("BenMods.Harmony");

       

        private static readonly KeybladeComboState testCombo =
            new KeybladeComboState();

        [HarmonyPostfix]
        public static void AfterUpdate(DebugMenu __instance)
        {


            // Only update diagnostics when the menu is visible.
            if (__instance.debugMenuObject == null ||
                !__instance.debugMenuObject.activeSelf ||
                __instance.playerCountText == null)
            {
                return;
            }

            __instance.playerCountText.verticalOverflow =
                VerticalWrapMode.Overflow;

            __instance.playerCountText.text +=
                DiagnosticRegistry.GetDisplayText();
                

             
        }
    }
}