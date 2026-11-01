using NUnit.Framework;
using Veyrath.Core;

namespace Veyrath.Tests
{
    public sealed class StoryFrameworkTests
    {
        private static QuestDefinition MainQuest()
        {
            return new QuestDefinition
            {
                id = "main-embers", title = "Embers of First Light", kind = QuestKind.Main,
                objectives = new[] { new QuestObjective { id = "meet-edda", description = "Meet Edda", requiredAmount = 1 } },
                consequenceFlags = new[] { "first-light.saved" }
            };
        }

        [Test]
        public void QuestLifecycleUnlocksByCompletionAndCreatesWorldConsequence()
        {
            StoryRuntime story = new StoryRuntime();
            story.RegisterQuest(MainQuest());
            QuestDefinition side = QuestGenerator.CreateFetchQuest("side-herbs", "Herbs", "moonleaf", 2);
            side.condition.requiredQuestId = "main-embers";
            story.RegisterQuest(side);
            Assert.That(story.StartQuest("main-embers"), Is.True);
            Assert.That(story.AdvanceObjective("main-embers", "meet-edda"), Is.True);
            Assert.That(story.CompleteQuest("main-embers"), Is.True);
            Assert.That(story.HasFlag("first-light.saved"), Is.True);
            Assert.That(story.GetQuest("side-herbs").progress, Is.EqualTo(QuestProgress.Available));
        }

        [Test]
        public void BranchChoiceRequiresConditionAndAppliesRelationshipAndFlag()
        {
            StoryRuntime story = new StoryRuntime();
            story.RegisterQuest(MainQuest());
            NpcSimulationService npcs = new NpcSimulationService();
            npcs.Register(new NpcProfile { id = "edda", displayName = "Edda" });
            DialogueChoice choice = new DialogueChoice { id = "promise", requiredWorldFlag = "trust", consequenceFlag = "promised", relationshipDelta = 10 };
            Assert.That(story.ApplyChoice(choice, npcs, "edda"), Is.False);
            story.SetFlag("trust");
            Assert.That(story.ApplyChoice(choice, npcs, "edda"), Is.True);
            Assert.That(story.HasFlag("promised"), Is.True);
            Assert.That(npcs.GetState("edda").relationship, Is.EqualTo(10));
        }

        [Test]
        public void CompanionRecruitmentRequiresCompletedQuest()
        {
            StoryRuntime story = new StoryRuntime();
            story.RegisterQuest(MainQuest());
            story.RegisterCompanion(new CompanionState { id = "lyra" });
            Assert.That(story.RecruitCompanion("lyra", "main-embers"), Is.False);
            story.StartQuest("main-embers"); story.AdvanceObjective("main-embers", "meet-edda"); story.CompleteQuest("main-embers");
            Assert.That(story.RecruitCompanion("lyra", "main-embers"), Is.True);
            Assert.That(story.Companions["lyra"].active, Is.True);
        }

        [Test]
        public void GeneratedQuestClampsObjectiveAmount()
        {
            QuestDefinition quest = QuestGenerator.CreateFetchQuest("side", "Side", "ore", 0);
            Assert.That(quest.objectives[0].requiredAmount, Is.EqualTo(1));
        }
    }
}
