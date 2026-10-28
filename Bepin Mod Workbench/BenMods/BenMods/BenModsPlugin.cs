using BepInEx;
using HarmonyLib;
using BenMods.Debugging;
using BenMods.Combat;
using UnityEngine;
using System.IO;
using BepInEx.Configuration;


namespace BenMods
{
    [BepInPlugin("com.benmods.tgp", "BenMods", "0.1.0")]
    public class BenModsPlugin : BaseUnityPlugin
    {
        internal static ConfigEntry<bool> VerboseCombatLogging;

        private void Awake()
        {
            Logger.LogInfo("BENMODS ONLINE!");
            
            VerboseCombatLogging = Config.Bind(
                "Debug",
                "VerboseCombatLogging",
                false,
                "Log every Keyblade combo preview and execution."
            );

            Harmony harmony = new Harmony("com.benmods.tgp");
            harmony.PatchAll();
            
            string expectedBundle = System.IO.Path.Combine(
                BenMods.Patches.StreamingAssetsPatch.ModDirectory,
                "AssetBundles",
                "weapons"
            );

            string discoveredBundle;

            bool found = StreamingAssets.TryGetFile(
                System.IO.Path.Combine("AssetBundles", "weapons"),
                out discoveredBundle
            );

            Logger.LogInfo(
                "BenMods directory: " +
                BenMods.Patches.StreamingAssetsPatch.ModDirectory
            );

            Logger.LogInfo(
                "Custom bundle exists: " +
                System.IO.File.Exists(expectedBundle)
            );

            Logger.LogInfo(
                "Discovered bundle: " + discoveredBundle
            );

           Logger.LogInfo(
                "Using custom bundle: " +
                (found &&
                string.Equals(
                    Path.GetFullPath(discoveredBundle),
                    Path.GetFullPath(expectedBundle),
                    System.StringComparison.OrdinalIgnoreCase
                ))
            );


            var target = AccessTools.Method(typeof(DebugMenu), "Update");
            var patchInfo = target != null
                ? Harmony.GetPatchInfo(target)
                : null;

            Logger.LogInfo("Harmony PatchAll completed.");
            Logger.LogInfo("Target found: " + (target != null));
            Logger.LogInfo("Postfix count: " + (patchInfo?.Postfixes.Count ?? 0));

            
            DiagnosticRegistry.Register(new WorldDiagnostics());
            Logger.LogInfo("World diagnostics registered.");
            DiagnosticRegistry.Register(new PlayerDiagnostics());
            Logger.LogInfo("Player diagnostics registered.");

           
        }
        
        

    }
}