
using HarmonyLib;
using BepInEx.Logging;
using BenMods.Combat;
using UnityEngine;

namespace BenMods.Patches
{
    [HarmonyPatch(
        typeof(Attacking.Ability),
        "ExecuteLocally"
    )]
    public static class AbilityExecutionPatch
    {
        private static readonly ManualLogSource Log =
            BepInEx.Logging.Logger.CreateLogSource(
                "BenMods.Combat"
            );

        [HarmonyPostfix]
        public static void AfterExecute(
            Attacking.Ability __instance,
            bool __result)
        {
            // Only successful basic attacks.
            if (!__result ||
                __instance.GetType().Name != "BasicAttack")
            {
                return;
            }

            var casterField = AccessTools.Field(
                typeof(Attacking.Ability),
                "caster"
            );

            if (casterField == null)
                return;

            Player player =
                casterField.GetValue(__instance) as Player;

            // Only Keybladekind advances this combo.
            if (player == null ||
                player.Networkweapon == null ||
                player.Networkweapon.WeaponKind.ToString()
                    != "Keyblade")
            {
                return;
            }

            KeybladeComboState state =
                KeybladeComboRegistry.Get(player);

            int step = state.CommitAttack(
                Time.realtimeSinceStartup
            );

            if (BenModsPlugin.VerboseCombatLogging.Value)
            {
                Log.LogInfo(
                    "KEYBLADE COMMITTED | Player: " +
                    player.name +
                    " | Step: " + step +
                    " | Animation: " +
                    KeybladeComboState.GetAnimation(step)
                );
            }
        }
    }
}
