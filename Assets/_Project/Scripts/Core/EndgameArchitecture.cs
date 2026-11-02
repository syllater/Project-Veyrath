using System;
using System.Collections.Generic;

namespace Veyrath.Core
{
    [Serializable]
    public sealed class BuildProfile
    {
        public string id;
        public int power;
        public int resilience;
        public int focus;
        public int unspentPoints;

        public bool Allocate(string stat, int amount)
        {
            if (amount <= 0 || amount > unspentPoints) return false;
            if (stat == "power") power = SafeAdd(power, amount);
            else if (stat == "resilience") resilience = SafeAdd(resilience, amount);
            else if (stat == "focus") focus = SafeAdd(focus, amount);
            else return false;
            unspentPoints -= amount; return true;
        }

        private static int SafeAdd(int value, int amount) { return (int)Math.Min(int.MaxValue, (long)Math.Max(0, value) + amount); }
    }

    [Serializable]
    public sealed class UniqueGearDefinition
    {
        public string id;
        public string displayName;
        public string setId;
        public int power;
        public bool unique = true;
    }

    [Serializable]
    public sealed class SetBonusDefinition
    {
        public string setId;
        public int requiredPieces;
        public int powerBonus;
        public string effectId;
    }

    public sealed class GearCollection
    {
        public readonly List<UniqueGearDefinition> Owned = new List<UniqueGearDefinition>();
        public readonly List<SetBonusDefinition> Bonuses = new List<SetBonusDefinition>();

        public bool Add(UniqueGearDefinition gear)
        {
            if (gear == null || string.IsNullOrWhiteSpace(gear.id) || Owned.Exists(item => item.id == gear.id)) return false;
            Owned.Add(gear); return true;
        }

        public int SetPower(string setId)
        {
            int pieces = Owned.FindAll(item => item.setId == setId).Count;
            int power = 0;
            foreach (SetBonusDefinition bonus in Bonuses) if (bonus.setId == setId && pieces >= bonus.requiredPieces) power += bonus.powerBonus;
            return power;
        }
    }

    [Serializable]
    public sealed class EndgameActivityState
    {
        public string id;
        public int tier;
        public int completions;
        public int longTermPoints;
    }

    public sealed class EndgameService
    {
        public readonly List<EndgameActivityState> Activities = new List<EndgameActivityState>();
        public readonly List<string> UnlockedRewards = new List<string>();

        public bool Complete(string id, int tier, int rewardPoints)
        {
            if (string.IsNullOrWhiteSpace(id) || tier <= 0 || rewardPoints <= 0) return false;
            EndgameActivityState activity = Activities.Find(item => item.id == id);
            if (activity == null) { activity = new EndgameActivityState { id = id }; Activities.Add(activity); }
            activity.tier = Math.Max(activity.tier, tier); activity.completions++; activity.longTermPoints = SafeAdd(activity.longTermPoints, rewardPoints);
            return true;
        }

        public bool UnlockReward(string rewardId, int cost, ref int points)
        {
            if (string.IsNullOrWhiteSpace(rewardId) || cost <= 0 || points < cost || UnlockedRewards.Contains(rewardId)) return false;
            points -= cost; UnlockedRewards.Add(rewardId); return true;
        }

        private static int SafeAdd(int value, int amount) { return (int)Math.Min(int.MaxValue, (long)Math.Max(0, value) + amount); }
    }

    public sealed class BossChallengeState
    {
        public string id;
        public int maxHealth;
        public int health;
        public int phase;
        public bool defeated;

        public BossChallengeState(string id, int health) { this.id = id; maxHealth = Math.Max(1, health); this.health = maxHealth; }
        public void ApplyDamage(int amount)
        {
            if (defeated || amount <= 0) return;
            health = Math.Max(0, health - amount); phase = health <= maxHealth / 2 ? 2 : 1; defeated = health == 0;
        }
    }

    public enum EndingType { Hope, Dominion, Exile }

    public static class EndingResolver
    {
        public static EndingType Resolve(bool savedSettlement, bool conqueredRegion, bool abandonedCompanions)
        {
            if (conqueredRegion && !savedSettlement) return EndingType.Dominion;
            if (abandonedCompanions) return EndingType.Exile;
            return EndingType.Hope;
        }
    }
}
