
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HarmonyLib;

namespace BenMods.Patches
{
    [HarmonyPatch]
    public static class StreamingAssetsPatch
    {
        public static string ModDirectory
        {
            get
            {
                return Path.GetDirectoryName(
                    typeof(BenModsPlugin).Assembly.Location
                );
            }
        }

        static MethodBase TargetMethod()
        {
            return AccessTools.PropertyGetter(
                typeof(StreamingAssets),
                "Paths"
            );
        }

        [HarmonyPostfix]
        static void AddModDirectory(
            ref IEnumerable<string> __result)
        {
            __result = IncludeModDirectory(__result);
        }

        private static IEnumerable<string> IncludeModDirectory(
            IEnumerable<string> originalPaths)
        {
            // Search our mod folder first.
            yield return ModDirectory;

            foreach (string path in originalPaths)
            {
                if (path != ModDirectory)
                    yield return path;
            }
        }
    }

    

}
