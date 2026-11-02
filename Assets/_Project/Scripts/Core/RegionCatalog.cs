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
                    streamDistance = 180f, lodBias = 1, connectedRegionIds = new[] { "ashen-reach" }, content = WorldContentCatalog.FirstLight
                }
            },
            {
                "ashen-reach", new RegionDefinition
                {
                    id = "ashen-reach", displayName = "Ashen Reach", sceneName = "AshenReach",
                    streamDistance = 220f, lodBias = 1, connectedRegionIds = new[] { "first-light", "verdant-reach" }, content = WorldContentCatalog.AshenReach
                }
            },
            {
                "verdant-reach", new RegionDefinition
                {
                    id = "verdant-reach", displayName = "Verdant Reach", sceneName = "VerdantReach",
                    streamDistance = 240f, lodBias = 1, connectedRegionIds = new[] { "ashen-reach", "frostmere" },
                    content = WorldContentCatalog.VerdantReach
                }
            },
            {
                "frostmere", new RegionDefinition
                {
                    id = "frostmere", displayName = "Frostmere", sceneName = "Frostmere",
                    streamDistance = 260f, lodBias = 1, connectedRegionIds = new[] { "verdant-reach", "ironroot" },
                    content = WorldContentCatalog.Frostmere
                }
            },
            {
                "ironroot", new RegionDefinition
                {
                    id = "ironroot", displayName = "Ironroot Basin", sceneName = "Ironroot",
                    streamDistance = 260f, lodBias = 1, connectedRegionIds = new[] { "frostmere", "starfall-desert" },
                    content = WorldContentCatalog.Ironroot
                }
            },
            {
                "starfall-desert", new RegionDefinition
                {
                    id = "starfall-desert", displayName = "Starfall Desert", sceneName = "StarfallDesert",
                    streamDistance = 300f, lodBias = 1, connectedRegionIds = new[] { "ironroot", "sunken-coast" },
                    content = WorldContentCatalog.StarfallDesert
                }
            },
            {
                "sunken-coast", new RegionDefinition
                {
                    id = "sunken-coast", displayName = "Sunken Coast", sceneName = "SunkenCoast",
                    streamDistance = 280f, lodBias = 1, connectedRegionIds = new[] { "starfall-desert", "veilwood" },
                    content = WorldContentCatalog.SunkenCoast
                }
            },
            {
                "veilwood", new RegionDefinition
                {
                    id = "veilwood", displayName = "Veilwood", sceneName = "Veilwood",
                    streamDistance = 240f, lodBias = 1, connectedRegionIds = new[] { "sunken-coast" },
                    content = WorldContentCatalog.Veilwood
                }
            }
        };

        public static IReadOnlyCollection<RegionDefinition> All { get { return Definitions.Values; } }

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
