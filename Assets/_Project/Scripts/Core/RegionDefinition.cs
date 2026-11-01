using System;

namespace Veyrath.Core
{
    [Serializable]
    public sealed class RegionDefinition
    {
        public string id;
        public string displayName;
        public string sceneName;
        public float streamDistance = 250f;
        public int lodBias = 1;
        public string[] connectedRegionIds = Array.Empty<string>();
    }
}
