
using HarmonyLib;
using BepInEx.Logging;

namespace BenMods.Patches
{
    [HarmonyPatch(typeof(DatabaseManager), "LoadOverlayDatabase")]
    public static class DatabaseOverlayPatch
    {
        private static readonly ManualLogSource Log =
            BepInEx.Logging.Logger.CreateLogSource(
                "BenMods.Items"
            );

        [HarmonyPostfix]
        static void AfterLoad(
            DatabaseManager __instance,
            string overlayName)
        {
            Log.LogWarning(
                "OVERLAY LOADED: " + overlayName
            );

            if (__instance.OverlayItems.TryGetValue(
                overlayName, out var items))
            {
                Log.LogInfo(
                    "ITEM COUNT: " + items.Count
                );

                foreach (var item in items)
                {
                    Log.LogInfo(
                        "REGISTERED ITEM: " +
                        item.Name +
                        " | Code: " + item.Code +
                        " | Kind: " + item.Strifekind
                    );
                }
            }

            

           
        }
    }
}
