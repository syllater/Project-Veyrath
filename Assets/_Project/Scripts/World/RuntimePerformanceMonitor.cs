using UnityEngine;
using UnityEngine.Profiling;
using Veyrath.Core;

namespace Veyrath.World
{
    public sealed class RuntimePerformanceMonitor : MonoBehaviour
    {
        public PerformanceSampleBuffer Samples { get; private set; } = new PerformanceSampleBuffer(120);
        public float CaptureIntervalSeconds = .5f;
        private float elapsed;
        private float lastCapture;

        private void Update()
        {
            elapsed += Time.unscaledDeltaTime;
            if (elapsed - lastCapture < CaptureIntervalSeconds) return;
            lastCapture = elapsed;
            Samples.Add(new PerformanceSample
            {
                frameMilliseconds = Time.unscaledDeltaTime * 1000f,
                managedMemoryBytes = System.GC.GetTotalMemory(false),
                allocatedMemoryBytes = Profiler.GetTotalAllocatedMemoryLong(),
                loadedRegions = FindAnyObjectByType<RegionStreamingManager>()?.LoadedRegions.Count ?? 0,
                simulatedNpcs = FindAnyObjectByType<NpcSimulationRuntime>()?.Service.States.Count ?? 0
            });
        }
    }
}
