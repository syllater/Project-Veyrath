using System;
using System.Collections.Generic;

namespace Veyrath.Core
{
    [Serializable]
    public sealed class DiscoveryState
    {
        public string id;
        public bool discovered;
    }

    [Serializable]
    public sealed class TravelAnchorState
    {
        public string id;
        public string regionId;
        public bool unlocked;
    }

    [Serializable]
    public sealed class WorldState
    {
        public string currentRegionId = "first-light";
        public int worldMinutes;
        public string weather = "clear";
        public List<DiscoveryState> discoveries = new List<DiscoveryState>();
        public List<TravelAnchorState> travelAnchors = new List<TravelAnchorState>();

        public bool EnterRegion(string regionId)
        {
            if (!RegionCatalog.AreConnected(currentRegionId, regionId)) return false;
            currentRegionId = regionId;
            AdvanceTime(15);
            return true;
        }

        public bool Discover(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            DiscoveryState entry = discoveries.Find(item => item.id == id);
            if (entry == null) discoveries.Add(new DiscoveryState { id = id, discovered = true });
            else if (entry.discovered) return false;
            else entry.discovered = true;
            return true;
        }

        public bool UnlockAnchor(string id, string regionId)
        {
            if (string.IsNullOrWhiteSpace(id) || RegionCatalog.Get(regionId) == null) return false;
            TravelAnchorState entry = travelAnchors.Find(item => item.id == id);
            if (entry != null) return false;
            travelAnchors.Add(new TravelAnchorState { id = id, regionId = regionId, unlocked = true });
            return true;
        }

        public void AdvanceTime(int minutes)
        {
            worldMinutes = (worldMinutes + Math.Max(0, minutes)) % (24 * 60);
        }

        public void SetWeather(string weatherId)
        {
            if (!string.IsNullOrWhiteSpace(weatherId)) weather = weatherId;
        }
    }
}
