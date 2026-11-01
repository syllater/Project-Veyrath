using System;
using System.Collections.Generic;

namespace Veyrath.Core
{
    [Serializable]
    public sealed class SkillEntry
    {
        public string id;
        public int rank;
    }

    [Serializable]
    public sealed class SkillState
    {
        public List<SkillEntry> skills = new List<SkillEntry>();

        public int Rank(string id)
        {
            SkillEntry entry = skills.Find(skill => skill.id == id);
            return entry == null ? 0 : entry.rank;
        }

        public bool Unlock(string id, CharacterProgression progression)
        {
            if (string.IsNullOrWhiteSpace(id) || progression == null || progression.unspentSkillPoints <= 0) return false;
            SkillEntry entry = skills.Find(skill => skill.id == id);
            if (entry == null)
            {
                entry = new SkillEntry { id = id, rank = 0 };
                skills.Add(entry);
            }
            entry.rank++;
            progression.unspentSkillPoints--;
            return true;
        }
    }
}
