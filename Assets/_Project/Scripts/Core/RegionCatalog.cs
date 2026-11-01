using System.Collections.Generic;

namespace Veyrath.Core
{
    public static class RegionCatalog
    {
        private static readonly Dictionary<string, RegionDefinition> Definitions = new Dictionary<string, RegionDefinition>
        {
            {
                "first-light", new RegionDefinition
                {
                    id = "first-light", displayName = "First Light", sceneName = "FirstLight",
                    streamDistance = 180f, lodBias = 1, connectedRegionIds = new[] { "ashen-reach" }
                }
            },
            {
                "ashen-reach", new RegionDefinition
                {
                    id = "ashen-reach", displayName = "Ashen Reach", sceneName = "AshenReach",
                    streamDistance = 220f, lodBias = 1, connectedRegionIds = new[] { "first-light" }
                }
            }
        };

        public static RegionDefinition Get(string id)
        {
            RegionDefinition definition;
            return id != null && Definitions.TryGetValue(id, out definition) ? definition : null;
        }

        public static bool AreConnected(string fromId, string toId)
        {
            RegionDefinition from = Get(fromId);
            if (from == null || toId == null) return false;
            for (int i = 0; i < from.connectedRegionIds.Length; i++)
                if (from.connectedRegionIds[i] == toId) return true;
            return false;
        }
    }
}
