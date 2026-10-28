using UnityEngine;
using System.Text;
using HarmonyLib;
using System.Reflection;

namespace BenMods.Debugging
{
    public class PlayerDiagnostics : IDiagnosticModule
    {
        public string Name => "PLAYER";

        private static readonly FieldInfo HealthField =
            AccessTools.Field(typeof(Attackable), "health");

        public string GetDisplayText()
        {
            Player[] players = Object.FindObjectsOfType<Player>();

            StringBuilder output = new StringBuilder();
            output.Append("Player objects: " + players.Length);

            foreach (Player player in players)
            {
                if (!player.isLocalPlayer)
                    continue;

                output.Append("\nLocal player: " + player.name);
                output.Append("\nOwned: " + player.hasAuthority);

                if (HealthField != null)
                {
                    float health = (float)HealthField.GetValue(player);
                    output.Append("\nHealth: " + health.ToString("F1"));
                }

                output.Append("\nPosition: " + player.transform.position);
            }

            return output.ToString();
        }
    }
}