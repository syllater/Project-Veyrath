using System;

namespace Veyrath.Core
{
    [Serializable]
    public sealed class CharacterProgression
    {
        public int level = 1;
        public int experience;
        public int unspentSkillPoints;

        public int ExperienceToNextLevel { get { return level * 100; } }

        public void AddExperience(int amount)
        {
            if (amount <= 0) return;
            experience += amount;
            while (experience >= ExperienceToNextLevel)
            {
                experience -= ExperienceToNextLevel;
                level++;
                unspentSkillPoints++;
            }
        }
    }
}
