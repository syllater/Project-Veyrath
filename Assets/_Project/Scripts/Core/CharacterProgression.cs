using System;

namespace Veyrath.Core
{
    [Serializable]
    public sealed class CharacterProgression
    {
        public const int MaxLevel = 4000;
        public int level = 1;
        public int experience;
        public int unspentSkillPoints;

        public int ExperienceToNextLevel { get { return level >= MaxLevel ? int.MaxValue : Math.Max(1, level * 100); } }

        public void AddExperience(int amount)
        {
            if (amount <= 0) return;
            if (level >= MaxLevel) { level = MaxLevel; experience = 0; return; }
            experience += amount;
            while (level < MaxLevel && experience >= ExperienceToNextLevel)
            {
                experience -= ExperienceToNextLevel;
                level++;
                unspentSkillPoints++;
            }
            if (level >= MaxLevel) { level = MaxLevel; experience = 0; }
        }
    }
}
