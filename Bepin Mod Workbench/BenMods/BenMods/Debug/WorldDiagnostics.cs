using UnityEngine;
using UnityEngine.SceneManagement;

namespace BenMods.Debugging
{
    public class WorldDiagnostics : IDiagnosticModule
    {
        public string Name
        {
            get { return "WORLD"; }
        }

        public string GetDisplayText()
        {
            Scene scene = SceneManager.GetActiveScene();

            return
                "Scene: " + scene.name +
                "\nTime: " + Time.time.ToString("F1") + "s" +
                "\nFPS: " + (1f / Mathf.Max(Time.unscaledDeltaTime, 0.0001f))
                    .ToString("F0");
        }
    }
}