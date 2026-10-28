
namespace BenMods.Combat
{
    public class KeybladeComboState
    {
        private const float ComboWindow = 1.5f;

        private int step = 0;
        private float lastAttackTime = -1f;

        // Preview the next attack without committing it.
        public int GetNextStep(float currentTime)
        {
            if (lastAttackTime < 0f ||
                currentTime < lastAttackTime ||
                currentTime - lastAttackTime > ComboWindow)
            {
                return 1;
            }

            return step % 3 + 1;
        }

        // Commit only after an attack succeeds.
        public int CommitAttack(float currentTime)
        {
            step = GetNextStep(currentTime);
            lastAttackTime = currentTime;

            return step;
        }

        public void Reset()
        {
            step = 0;
            lastAttackTime = -1f;
        }

        public int CurrentStep
        {
            get { return step; }
        }

        public static string GetAnimation(int attackStep)
        {
            switch (attackStep)
            {
                case 1: return "Hit";
                case 2: return "Stab";
                case 3: return "Twohandattack";
                default: return "Hit";
            }
        }
    }
}
