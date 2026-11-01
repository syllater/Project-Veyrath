using System;

namespace Veyrath.Core
{
    [Serializable]
    public sealed class NpcState
    {
        public string id;
        public string regionId;
        public string activity;
        public int relationship;
        public int lastSimulatedMinute;

        public string ContextLine(NpcProfile profile)
        {
            if (relationship >= 20) return profile.displayName + ": I knew I could count on you.";
            if (activity == "sleep") return profile.displayName + " is resting and will speak at dawn.";
            if (activity == "work") return profile.displayName + " is working at the camp.";
            return profile.displayName + ": The road is quiet for now.";
        }
    }
}
