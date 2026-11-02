using System;

namespace Veyrath.Core
{
    [Serializable]
    public sealed class RegionContentDefinition
    {
        public string[] settlements = Array.Empty<string>();
        public string[] ruins = Array.Empty<string>();
        public string[] dungeons = Array.Empty<string>();
        public string[] quests = Array.Empty<string>();
        public string[] npcs = Array.Empty<string>();
        public string[] factions = Array.Empty<string>();
        public string[] bosses = Array.Empty<string>();
        public string[] items = Array.Empty<string>();
        public string[] resources = Array.Empty<string>();
        public string[] secrets = Array.Empty<string>();
        public string[] worldEvents = Array.Empty<string>();
    }

    [Serializable]
    public sealed class RegionDefinition
    {
        public string id;
        public string displayName;
        public string sceneName;
        public float streamDistance = 250f;
        public int lodBias = 1;
        public string[] connectedRegionIds = Array.Empty<string>();
        public RegionContentDefinition content = new RegionContentDefinition();
    }
}
