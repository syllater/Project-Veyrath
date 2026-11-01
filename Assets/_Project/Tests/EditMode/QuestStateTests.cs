using NUnit.Framework;
using Veyrath.Core;

namespace Veyrath.Tests
{
    public sealed class QuestStateTests
    {
        [Test]
        public void QuestMustBeAcceptedBeforeWraithCanBeDefeated()
        {
            QuestState quest = new QuestState();

            Assert.That(quest.CompleteWraithHunt(), Is.False);
            Assert.That(quest.Accept(), Is.True);
            Assert.That(quest.CompleteWraithHunt(), Is.True);
            Assert.That(quest.stage, Is.EqualTo(FirstLightQuestStage.ReturnToEdda));
        }

        [Test]
        public void RewardCanOnlyBeClaimedOnce()
        {
            QuestState quest = new QuestState();
            quest.Accept();
            quest.CompleteWraithHunt();

            Assert.That(quest.ClaimReward(), Is.True);
            Assert.That(quest.ClaimReward(), Is.False);
            Assert.That(quest.stage, Is.EqualTo(FirstLightQuestStage.Complete));
        }
    }
}
