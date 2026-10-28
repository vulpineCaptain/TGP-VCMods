
using System.Collections.Generic;

namespace BenMods.Combat
{
    public static class KeybladeComboRegistry
    {
        private static readonly Dictionary<Player, KeybladeComboState> States =
            new Dictionary<Player, KeybladeComboState>();

        public static KeybladeComboState Get(Player player)
        {
            if (!States.TryGetValue(player, out KeybladeComboState state))
            {
                state = new KeybladeComboState();
                States[player] = state;
            }

            return state;
        }

        public static void Reset(Player player)
        {
            if (player != null)
                States.Remove(player);
        }
    }
}
