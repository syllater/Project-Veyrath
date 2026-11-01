using System;

namespace Veyrath.Core
{
    [Serializable]
    public sealed class NpcRoutineSegment
    {
        public int startMinute;
        public int endMinute;
        public string activity;
        public string regionId;
    }

    [Serializable]
    public sealed class NpcProfile
    {
        public string id;
        public string displayName;
        public string factionId;
        public string homeRegionId;
        public NpcRoutineSegment[] routine = Array.Empty<NpcRoutineSegment>();
    }
}
