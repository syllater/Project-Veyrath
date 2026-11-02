using NUnit.Framework;
using Veyrath.Core;

namespace Veyrath.Tests
{
    public sealed class EndgameArchitectureTests
    {
        [Test]
        public void ProgressionReachesLevel4000WithoutOverflow()
        {
            var progression = new CharacterProgression(); progression.AddExperience(int.MaxValue);
            Assert.That(progression.level, Is.EqualTo(CharacterProgression.MaxLevel));
            progression.AddExperience(int.MaxValue);
            Assert.That(progression.level, Is.EqualTo(4000)); Assert.That(progression.experience, Is.Zero);
        }

        [Test]
        public void BuildProfileAllocatesStatsAndRejectsUnknownValues()
        {
            var build = new BuildProfile { id = "warden", unspentPoints = 10 };
            Assert.That(build.Allocate("resilience", 6), Is.True); Assert.That(build.resilience, Is.EqualTo(6));
            Assert.That(build.Allocate("luck", 1), Is.False); Assert.That(build.unspentPoints, Is.EqualTo(4));
        }

        [Test]
        public void UniqueGearAndSetBonusScaleBuildPower()
        {
            var gear = new GearCollection(); gear.Bonuses.Add(new SetBonusDefinition { setId = "dawn", requiredPieces = 2, powerBonus = 25 });
            Assert.That(gear.Add(new UniqueGearDefinition { id = "helm", setId = "dawn" }), Is.True);
            Assert.That(gear.Add(new UniqueGearDefinition { id = "helm", setId = "dawn" }), Is.False);
            gear.Add(new UniqueGearDefinition { id = "chest", setId = "dawn" });
            Assert.That(gear.SetPower("dawn"), Is.EqualTo(25));
        }

        [Test]
        public void EndgameActivityTracksLongTermPointsAndRewards()
        {
            var service = new EndgameService(); int points = 100;
            Assert.That(service.Complete("rift", 3, 40), Is.True); Assert.That(service.Complete("rift", 3, 40), Is.True);
            Assert.That(service.UnlockReward("title-ashenbreaker", 75, ref points), Is.True);
            Assert.That(points, Is.EqualTo(25)); Assert.That(service.Activities[0].completions, Is.EqualTo(2));
        }

        [Test]
        public void BossChallengeHasPhaseAndDefeatState()
        {
            var boss = new BossChallengeState("hollow-queen", 1000); boss.ApplyDamage(501);
            Assert.That(boss.phase, Is.EqualTo(2)); Assert.That(boss.defeated, Is.False);
            boss.ApplyDamage(499); Assert.That(boss.defeated, Is.True);
        }

        [Test]
        public void AlternativeEndingsFollowWorldConsequences()
        {
            Assert.That(EndingResolver.Resolve(true, false, false), Is.EqualTo(EndingType.Hope));
            Assert.That(EndingResolver.Resolve(false, true, false), Is.EqualTo(EndingType.Dominion));
            Assert.That(EndingResolver.Resolve(true, false, true), Is.EqualTo(EndingType.Exile));
        }
    }
}
