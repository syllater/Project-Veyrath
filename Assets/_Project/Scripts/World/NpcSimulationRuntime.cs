using UnityEngine;
using Veyrath.Core;

namespace Veyrath.World
{
    public sealed class NpcSimulationRuntime : MonoBehaviour
    {
        public NpcSimulationService Service { get; private set; } = new NpcSimulationService();
        private RegionStreamingManager regions;

        public void Initialize(RegionStreamingManager regionManager)
        {
            regions = regionManager;
            Service.Register(new NpcProfile
            {
                id = "edda", displayName = "Edda", factionId = "first-light-wardens", homeRegionId = "first-light",
                routine = new[]
                {
                    new NpcRoutineSegment { startMinute = 0, endMinute = 360, activity = "sleep", regionId = "first-light" },
                    new NpcRoutineSegment { startMinute = 360, endMinute = 1080, activity = "work", regionId = "first-light" },
                    new NpcRoutineSegment { startMinute = 1080, endMinute = 1440, activity = "patrol", regionId = "first-light" }
                }
            });
            Service.SimulateTo(regions.State.worldMinutes);
        }

        private void Update()
        {
            if (regions != null && Service.SimulateOffscreen) Service.SimulateTo(regions.State.worldMinutes);
        }
    }
}
