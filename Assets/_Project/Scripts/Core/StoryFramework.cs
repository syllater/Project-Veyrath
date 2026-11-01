using System;
using System.Collections.Generic;

namespace Veyrath.Core
{
    public enum QuestKind { Main, Side }
    public enum QuestProgress { Locked, Available, Active, Completed, Failed }

    [Serializable]
    public sealed class QuestCondition
    {
        public string requiredQuestId = string.Empty;
        public string requiredWorldFlag = string.Empty;
        public int minimumRelationship;
    }

    [Serializable]
    public sealed class QuestObjective
    {
        public string id;
        public string description;
        public int requiredAmount = 1;
    }

    [Serializable]
    public sealed class QuestDefinition
    {
        public string id;
        public string title;
        public QuestKind kind;
        public QuestCondition condition = new QuestCondition();
        public QuestObjective[] objectives = Array.Empty<QuestObjective>();
        public string[] consequenceFlags = Array.Empty<string>();
    }

    [Serializable]
    public sealed class QuestObjectiveProgress
    {
        public string objectiveId;
        public int amount;
    }

    [Serializable]
    public sealed class QuestRuntimeState
    {
        public string questId;
        public QuestProgress progress;
        public List<QuestObjectiveProgress> objectiveProgress = new List<QuestObjectiveProgress>();
    }

    [Serializable]
    public sealed class DialogueChoice
    {
        public string id;
        public string text;
        public string nextNodeId;
        public string requiredWorldFlag = string.Empty;
        public string consequenceFlag = string.Empty;
        public int relationshipDelta;
    }

    [Serializable]
    public sealed class DialogueNode
    {
        public string id;
        public string speaker;
        public string text;
        public DialogueChoice[] choices = Array.Empty<DialogueChoice>();
    }

    [Serializable]
    public sealed class CompanionState
    {
        public string id;
        public bool recruited;
        public bool active;
        public string questId;
    }

    public sealed class StoryRuntime
    {
        private readonly Dictionary<string, QuestDefinition> definitions = new Dictionary<string, QuestDefinition>();
        private readonly Dictionary<string, QuestRuntimeState> states = new Dictionary<string, QuestRuntimeState>();
        private readonly HashSet<string> worldFlags = new HashSet<string>();
        private readonly Dictionary<string, CompanionState> companions = new Dictionary<string, CompanionState>();

        public IReadOnlyCollection<string> WorldFlags { get { return worldFlags; } }
        public IReadOnlyDictionary<string, QuestRuntimeState> Quests { get { return states; } }
        public IReadOnlyDictionary<string, CompanionState> Companions { get { return companions; } }

        public void RegisterQuest(QuestDefinition definition)
        {
            if (definition == null || string.IsNullOrWhiteSpace(definition.id)) return;
            definitions[definition.id] = definition;
            if (!states.ContainsKey(definition.id)) states[definition.id] = new QuestRuntimeState { questId = definition.id, progress = QuestProgress.Locked };
            RefreshAvailability();
        }

        public void RegisterCompanion(CompanionState companion)
        {
            if (companion == null || string.IsNullOrWhiteSpace(companion.id)) return;
            companions[companion.id] = companion;
        }

        public QuestRuntimeState GetQuest(string id) { QuestRuntimeState state; return states.TryGetValue(id, out state) ? state : null; }
        public bool HasFlag(string flag) { return !string.IsNullOrWhiteSpace(flag) && worldFlags.Contains(flag); }
        public bool SetFlag(string flag) { return !string.IsNullOrWhiteSpace(flag) && worldFlags.Add(flag); }

        public bool StartQuest(string id)
        {
            QuestRuntimeState state = GetQuest(id);
            if (state == null || state.progress != QuestProgress.Available) return false;
            state.progress = QuestProgress.Active;
            return true;
        }

        public bool AdvanceObjective(string questId, string objectiveId, int amount = 1)
        {
            QuestRuntimeState state = GetQuest(questId);
            QuestDefinition definition;
            if (state == null || !definitions.TryGetValue(questId, out definition) || state.progress != QuestProgress.Active || amount <= 0) return false;
            QuestObjective objective = Array.Find(definition.objectives, item => item != null && item.id == objectiveId);
            if (objective == null) return false;
            QuestObjectiveProgress progress = state.objectiveProgress.Find(item => item.objectiveId == objectiveId);
            if (progress == null) { progress = new QuestObjectiveProgress { objectiveId = objectiveId }; state.objectiveProgress.Add(progress); }
            progress.amount = Math.Min(objective.requiredAmount, progress.amount + amount);
            return true;
        }

        public bool CompleteQuest(string id)
        {
            QuestRuntimeState state = GetQuest(id);
            QuestDefinition definition;
            if (state == null || !definitions.TryGetValue(id, out definition) || state.progress != QuestProgress.Active || !ObjectivesComplete(definition, state)) return false;
            state.progress = QuestProgress.Completed;
            foreach (string flag in definition.consequenceFlags) SetFlag(flag);
            RefreshAvailability();
            return true;
        }

        public bool ObjectivesComplete(QuestDefinition definition, QuestRuntimeState state)
        {
            if (definition == null || state == null) return false;
            foreach (QuestObjective objective in definition.objectives)
            {
                QuestObjectiveProgress progress = objective == null ? null : state.objectiveProgress.Find(item => item.objectiveId == objective.id);
                if (objective == null || progress == null || progress.amount < objective.requiredAmount) return false;
            }
            return true;
        }

        public void RefreshAvailability()
        {
            foreach (KeyValuePair<string, QuestDefinition> pair in definitions)
            {
                QuestRuntimeState state = states[pair.Key];
                if (state.progress != QuestProgress.Locked) continue;
                QuestCondition condition = pair.Value.condition ?? new QuestCondition();
                bool questOk = string.IsNullOrWhiteSpace(condition.requiredQuestId) || (GetQuest(condition.requiredQuestId) != null && GetQuest(condition.requiredQuestId).progress == QuestProgress.Completed);
                bool flagOk = string.IsNullOrWhiteSpace(condition.requiredWorldFlag) || HasFlag(condition.requiredWorldFlag);
                if (questOk && flagOk) state.progress = QuestProgress.Available;
            }
        }

        public bool RecruitCompanion(string id, string questId)
        {
            CompanionState companion;
            QuestRuntimeState quest = GetQuest(questId);
            if (!companions.TryGetValue(id, out companion) || quest == null || quest.progress != QuestProgress.Completed) return false;
            companion.recruited = true; companion.active = true; companion.questId = questId;
            return true;
        }

        public bool ApplyChoice(DialogueChoice choice, NpcSimulationService npcSimulation, string npcId)
        {
            if (choice == null || (!string.IsNullOrWhiteSpace(choice.requiredWorldFlag) && !HasFlag(choice.requiredWorldFlag))) return false;
            SetFlag(choice.consequenceFlag);
            if (npcSimulation != null && !string.IsNullOrWhiteSpace(npcId)) npcSimulation.AdjustRelationship(npcId, choice.relationshipDelta);
            return true;
        }
    }

    public static class QuestGenerator
    {
        public static QuestDefinition CreateFetchQuest(string id, string title, string itemId, int amount, QuestKind kind = QuestKind.Side)
        {
            return new QuestDefinition
            {
                id = id, title = title, kind = kind,
                objectives = new[] { new QuestObjective { id = "collect-" + itemId, description = "Collect " + itemId, requiredAmount = Math.Max(1, amount) } },
                consequenceFlags = new[] { id + ".resolved" }
            };
        }
    }
}
