using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Veyrath.Core;

namespace Veyrath.World
{
    public sealed class RegionStreamingManager : MonoBehaviour
    {
        public WorldState State { get; private set; } = new WorldState();
        private readonly HashSet<string> loadedRegions = new HashSet<string> { "first-light" };

        public IReadOnlyCollection<string> LoadedRegions { get { return loadedRegions; } }

        public bool EnterRegion(string regionId)
        {
            RegionDefinition target = RegionCatalog.Get(regionId);
            if (target == null || !State.EnterRegion(regionId)) return false;
            loadedRegions.Add(regionId);
            return true;
        }

        public bool LeaveRegion(string regionId)
        {
            if (regionId == State.currentRegionId || regionId == "first-light") return false;
            return loadedRegions.Remove(regionId);
        }

        public void Discover(string id) { State.Discover(id); }
        public void UnlockAnchor(string id) { State.UnlockAnchor(id, State.currentRegionId); }
        public void AdvanceTime(int minutes) { State.AdvanceTime(minutes); }
        public void SetWeather(string weatherId) { State.SetWeather(weatherId); }

        public void ApplyLod(LODGroup group, RegionDefinition definition)
        {
            if (group == null || definition == null) return;
            group.fadeMode = LODFadeMode.CrossFade;
            group.animateCrossFading = definition.lodBias > 0;
        }

        public bool TryLoadSceneForRegion(string regionId)
        {
            RegionDefinition definition = RegionCatalog.Get(regionId);
            if (definition == null || !State.EnterRegion(regionId)) return false;
            if (loadedRegions.Contains(regionId)) return true;
            loadedRegions.Add(regionId);
            if (!Application.CanStreamedLevelBeLoaded(definition.sceneName)) return true;
            SceneManager.LoadSceneAsync(definition.sceneName, LoadSceneMode.Additive);
            return true;
        }
    }
}
