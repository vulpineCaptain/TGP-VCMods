
using System.Reflection;
using HarmonyLib;
using BepInEx.Logging;
using BenMods.Combat;
using UnityEngine;

namespace BenMods.Patches
{
    [HarmonyPatch]
    public static class KeybladeEffectPatch
    {
        private static readonly ManualLogSource Log =
            BepInEx.Logging.Logger.CreateLogSource(
                "BenMods.Keyblade"
            );

        // BasicAttack is a non-public nested class.
        static MethodBase TargetMethod()
        {
            var type = AccessTools.Inner(
                typeof(Attacking),
                "BasicAttack"
            );

            return AccessTools.Method(type, "Effect");
        }

        [HarmonyPrefix]
        static void BeforeEffect(object __instance)
        {
            var ability = __instance as Attacking.Ability;

            if (ability == null)
                return;

            var casterField = AccessTools.Field(
                typeof(Attacking.Ability),
                "caster"
            );

            if (casterField == null)
                return;

            Player player =
                casterField.GetValue(ability) as Player;

            if (player == null ||
                player.Networkweapon == null ||
                player.Networkweapon.WeaponKind.ToString()
                    != "Keyblade")
            {
                return;
            }

            KeybladeComboState state =
                KeybladeComboRegistry.Get(player);

            int nextStep = state.GetNextStep(
                Time.realtimeSinceStartup
            );

            string animation =
                KeybladeComboState.GetAnimation(nextStep);

            // Select the animation BEFORE ExecuteLocally
            // calls animator.Play().
            ability.animation = animation;
            if (BenModsPlugin.VerboseCombatLogging.Value)
            {
                    
                Log.LogInfo(
                    "KEYBLADE PREVIEW | Player: " +
                    player.name +
                    " | Step: " + nextStep +
                    " | Animation: " + animation
                    );
                
            }
        }
    }

    // Reset combo progress when a weapon changes.
    [HarmonyPatch(typeof(Player), "OnWeaponChanged")]
    public static class KeybladeWeaponChangePatch
    {
        [HarmonyPostfix]
        static void AfterWeaponChanged(Player __instance)
        {
            KeybladeComboRegistry.Reset(__instance);
        }
    }
}
