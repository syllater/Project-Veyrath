using System;
using System.Collections.Generic;

namespace Veyrath.Core
{
    public enum WeaponType { Sword, Bow, Staff, Hammer }

    [Serializable]
    public sealed class WeaponDefinition
    {
        public string id;
        public string displayName;
        public WeaponType type;
        public int baseDamage;
        public float staminaCost;
        public float cooldown;
        public string statusEffectId;

        public WeaponDefinition(string id, string displayName, WeaponType type, int baseDamage, float staminaCost, float cooldown, string statusEffectId = "")
        {
            this.id = id; this.displayName = displayName; this.type = type; this.baseDamage = baseDamage;
            this.staminaCost = staminaCost; this.cooldown = cooldown; this.statusEffectId = statusEffectId ?? string.Empty;
        }
    }

    public static class WeaponCatalog
    {
        public static List<WeaponDefinition> CreateDefaults()
        {
            return new List<WeaponDefinition>
            {
                new WeaponDefinition("ashen-shortblade", "Ashen shortblade", WeaponType.Sword, 25, 8f, .55f),
                new WeaponDefinition("thornbow", "Thornbow", WeaponType.Bow, 19, 12f, .8f),
                new WeaponDefinition("dawnstaff", "Dawnstaff", WeaponType.Staff, 22, 10f, .7f, "scorch"),
                new WeaponDefinition("warding-hammer", "Warding hammer", WeaponType.Hammer, 31, 18f, 1.05f)
            };
        }
    }

    public static class CombatRules
    {
        public static int ResolveDamage(WeaponDefinition weapon, int skillRank, float difficultyMultiplier)
        {
            if (weapon == null) return 0;
            float value = Math.Max(1, weapon.baseDamage + Math.Max(0, skillRank) * 5) * Math.Max(.1f, difficultyMultiplier);
            return Math.Max(1, (int)Math.Round(value));
        }
    }

    [Serializable]
    public sealed class StatusEffectState
    {
        public string id;
        public float remainingSeconds;
        public float intensity;
    }

    public sealed class StatusEffectService
    {
        private readonly List<StatusEffectState> active = new List<StatusEffectState>();
        public IReadOnlyList<StatusEffectState> Active { get { return active; } }

        public void Apply(string id, float durationSeconds, float intensity)
        {
            if (string.IsNullOrWhiteSpace(id) || durationSeconds <= 0f) return;
            StatusEffectState state = active.Find(item => item.id == id);
            if (state == null) active.Add(new StatusEffectState { id = id, remainingSeconds = durationSeconds, intensity = Math.Max(0f, intensity) });
            else { state.remainingSeconds = Math.Max(state.remainingSeconds, durationSeconds); state.intensity = Math.Max(state.intensity, intensity); }
        }

        public float Tick(float seconds)
        {
            if (seconds <= 0f) return 0f;
            float damage = 0f;
            for (int i = active.Count - 1; i >= 0; i--)
            {
                StatusEffectState state = active[i];
                damage += state.intensity * Math.Min(seconds, state.remainingSeconds);
                state.remainingSeconds -= seconds;
                if (state.remainingSeconds <= 0f) active.RemoveAt(i);
            }
            return damage;
        }
    }

    public enum EnemyRole { Bruiser, Skirmisher, Ranged, Support }
    public enum AdaptiveTactic { Advance, Strafe, Retreat, Protect, Counter }

    [Serializable]
    public sealed class EnemyCombatProfile
    {
        public string id;
        public EnemyRole role;
        public int maxHealth;
        public int baseDamage;
        public EnemyCombatProfile(string id, EnemyRole role, int maxHealth, int baseDamage)
        { this.id = id; this.role = role; this.maxHealth = maxHealth; this.baseDamage = baseDamage; }
    }

    public sealed class AdaptiveTactics
    {
        private readonly Dictionary<WeaponType, int> seen = new Dictionary<WeaponType, int>();
        private WeaponType lastType;
        private int repeated;

        public void Observe(WeaponType type)
        {
            if (type == lastType) repeated++; else { lastType = type; repeated = 1; }
            seen[type] = seen.ContainsKey(type) ? seen[type] + 1 : 1;
        }

        public AdaptiveTactic Choose(EnemyCombatProfile profile, float distance, bool allyNeedsProtection)
        {
            if (profile == null) return AdaptiveTactic.Advance;
            if (profile.role == EnemyRole.Support && allyNeedsProtection) return AdaptiveTactic.Protect;
            if (profile.role == EnemyRole.Ranged) return distance < 4f ? AdaptiveTactic.Retreat : AdaptiveTactic.Strafe;
            if (repeated >= 3) return AdaptiveTactic.Counter;
            return distance > 5f ? AdaptiveTactic.Advance : AdaptiveTactic.Strafe;
        }
    }

    [Serializable]
    public sealed class BossPhaseDefinition
    {
        public string id;
        public float healthThreshold;
        public float damageMultiplier;
        public AdaptiveTactic tactic;
        public BossPhaseDefinition(string id, float healthThreshold, float damageMultiplier, AdaptiveTactic tactic)
        { this.id = id; this.healthThreshold = healthThreshold; this.damageMultiplier = damageMultiplier; this.tactic = tactic; }
    }

    public sealed class BossState
    {
        public readonly string bossId;
        public readonly List<BossPhaseDefinition> phases;
        public BossState(string bossId, List<BossPhaseDefinition> phases) { this.bossId = bossId; this.phases = phases ?? new List<BossPhaseDefinition>(); }
        public BossPhaseDefinition CurrentPhase(float currentHealth, float maxHealth)
        {
            float ratio = maxHealth <= 0f ? 0f : currentHealth / maxHealth;
            BossPhaseDefinition selected = null;
            float nearestThreshold = float.MaxValue;
            foreach (BossPhaseDefinition phase in phases)
                if (phase != null && ratio <= phase.healthThreshold && phase.healthThreshold < nearestThreshold)
                { selected = phase; nearestThreshold = phase.healthThreshold; }
            if (selected == null && phases.Count > 0) selected = phases[phases.Count - 1];
            return selected;
        }
    }

    public sealed class DifficultySettings
    {
        public readonly string id;
        public readonly float enemyHealthMultiplier;
        public readonly float enemyDamageMultiplier;
        public DifficultySettings(string id, float health, float damage) { this.id = id; enemyHealthMultiplier = health; enemyDamageMultiplier = damage; }
        public static DifficultySettings Normal { get { return new DifficultySettings("normal", 1f, 1f); } }
        public static DifficultySettings Story { get { return new DifficultySettings("story", .75f, .7f); } }
        public static DifficultySettings Veteran { get { return new DifficultySettings("veteran", 1.35f, 1.25f); } }
    }

    public sealed class BalanceTelemetry
    {
        public int attacks;
        public int totalDamage;
        public void RecordAttack(int damage) { attacks++; totalDamage += Math.Max(0, damage); }
        public float AverageDamage { get { return attacks == 0 ? 0f : (float)totalDamage / attacks; } }
    }
}
