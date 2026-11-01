using NUnit.Framework;
using Veyrath.Core;

namespace Veyrath.Tests
{
    public sealed class CombatArchitectureTests
    {
        [Test] public void WeaponCatalogProvidesDistinctCombatStyles()
        {
            var weapons = WeaponCatalog.CreateDefaults();
            Assert.That(weapons.Count, Is.GreaterThanOrEqualTo(3));
            Assert.That(weapons.Exists(w => w.type == WeaponType.Sword), Is.True);
            Assert.That(weapons.Exists(w => w.type == WeaponType.Bow), Is.True);
            Assert.That(weapons.Exists(w => w.type == WeaponType.Staff), Is.True);
        }

        [Test] public void StatusEffectsStackDurationAndExpire()
        {
            var effects = new StatusEffectService();
            effects.Apply("burn", 3f, 4f); effects.Apply("burn", 5f, 2f);
            Assert.That(effects.Active.Count, Is.EqualTo(1));
            Assert.That(effects.Tick(2f), Is.EqualTo(8f).Within(.01f));
            effects.Tick(4f);
            Assert.That(effects.Active.Count, Is.Zero);
        }

        [Test] public void AdaptiveTacticsCounterRepeatedWeaponAndProtectsAllies()
        {
            var tactics = new AdaptiveTactics();
            for (int i = 0; i < 3; i++) tactics.Observe(WeaponType.Sword);
            Assert.That(tactics.Choose(new EnemyCombatProfile("w", EnemyRole.Bruiser, 50, 8), 3f, false), Is.EqualTo(AdaptiveTactic.Counter));
            Assert.That(tactics.Choose(new EnemyCombatProfile("s", EnemyRole.Support, 30, 4), 3f, true), Is.EqualTo(AdaptiveTactic.Protect));
        }

        [Test] public void BossPhaseChangesAtHealthThreshold()
        {
            var boss = new BossState("ashen-wraith", new System.Collections.Generic.List<BossPhaseDefinition>
            { new BossPhaseDefinition("one", 1f, 1f, AdaptiveTactic.Advance), new BossPhaseDefinition("two", .5f, 1.5f, AdaptiveTactic.Counter) });
            Assert.That(boss.CurrentPhase(40f, 100f).id, Is.EqualTo("two"));
            Assert.That(boss.CurrentPhase(80f, 100f).id, Is.EqualTo("one"));
        }

        [Test] public void BossPhaseSelectionIsStableWhenDefinitionsAreUnordered()
        {
            var boss = new BossState("wraith", new System.Collections.Generic.List<BossPhaseDefinition>
            { new BossPhaseDefinition("enrage", .5f, 1.5f, AdaptiveTactic.Counter), new BossPhaseDefinition("opening", 1f, 1f, AdaptiveTactic.Advance) });
            Assert.That(boss.CurrentPhase(40f, 100f).id, Is.EqualTo("enrage"));
        }

        [Test] public void DifficultyAndTelemetryProduceBalanceableNumbers()
        {
            var weapon = WeaponCatalog.CreateDefaults()[0];
            int normal = CombatRules.ResolveDamage(weapon, 1, DifficultySettings.Normal.enemyDamageMultiplier);
            int veteran = CombatRules.ResolveDamage(weapon, 1, DifficultySettings.Veteran.enemyDamageMultiplier);
            var telemetry = new BalanceTelemetry(); telemetry.RecordAttack(normal); telemetry.RecordAttack(veteran);
            Assert.That(veteran, Is.GreaterThan(normal));
            Assert.That(telemetry.AverageDamage, Is.EqualTo((normal + veteran) / 2f).Within(.01f));
        }
    }
}
