using System;

namespace Veyrath.Core
{
    public enum FirstLightQuestStage
    {
        NotStarted,
        HuntTheWraith,
        ReturnToEdda,
        Complete
    }

    [Serializable]
    public sealed class QuestState
    {
        public FirstLightQuestStage stage;
        public bool rewardClaimed;

        public bool Accept()
        {
            if (stage != FirstLightQuestStage.NotStarted) return false;
            stage = FirstLightQuestStage.HuntTheWraith;
            return true;
        }

        public bool CompleteWraithHunt()
        {
            if (stage != FirstLightQuestStage.HuntTheWraith) return false;
            stage = FirstLightQuestStage.ReturnToEdda;
            return true;
        }

        public bool ClaimReward()
        {
            if (stage != FirstLightQuestStage.ReturnToEdda || rewardClaimed) return false;
            rewardClaimed = true;
            stage = FirstLightQuestStage.Complete;
            return true;
        }
    }
}
