using System;
using System.Collections.Generic;

namespace Veyrath.Core
{
    public sealed class ObjectPool<T> where T : class
    {
        private readonly Stack<T> available = new Stack<T>();
        private readonly Func<T> factory;
        private readonly Action<T> reset;
        public int Created { get; private set; }
        public int Available { get { return available.Count; } }

        public ObjectPool(Func<T> factory, Action<T> reset = null)
        { this.factory = factory ?? throw new ArgumentNullException("factory"); this.reset = reset; }

        public T Rent()
        {
            if (available.Count > 0) return available.Pop();
            Created++; return factory();
        }

        public void Return(T item)
        {
            if (item == null) return;
            reset?.Invoke(item); available.Push(item);
        }
    }

    public struct PerformanceSample
    {
        public float frameMilliseconds;
        public long managedMemoryBytes;
        public long allocatedMemoryBytes;
        public int loadedRegions;
        public int simulatedNpcs;
    }

    public sealed class PerformanceSampleBuffer
    {
        private readonly PerformanceSample[] samples;
        private int next;
        private int count;
        public int Count { get { return count; } }

        public PerformanceSampleBuffer(int capacity)
        { samples = new PerformanceSample[Math.Max(1, capacity)]; }

        public void Add(PerformanceSample sample)
        { samples[next] = sample; next = (next + 1) % samples.Length; count = Math.Min(samples.Length, count + 1); }

        public PerformanceSample Latest { get { return count == 0 ? default(PerformanceSample) : samples[(next - 1 + samples.Length) % samples.Length]; } }
    }
}
