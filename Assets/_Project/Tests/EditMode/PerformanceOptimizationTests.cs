using NUnit.Framework;
using Veyrath.Core;

namespace Veyrath.Tests
{
    public sealed class PerformanceOptimizationTests
    {
        private sealed class PooledItem { public int value; }

        [Test]
        public void ObjectPoolReusesInstancesAndResetsState()
        {
            var pool = new ObjectPool<PooledItem>(() => new PooledItem(), item => item.value = 0);
            PooledItem first = pool.Rent(); first.value = 9; pool.Return(first);
            PooledItem second = pool.Rent();
            Assert.That(object.ReferenceEquals(first, second), Is.True);
            Assert.That(second.value, Is.Zero); Assert.That(pool.Created, Is.EqualTo(1));
        }

        [Test]
        public void SampleBufferIsBoundedAndKeepsLatestMeasurement()
        {
            var buffer = new PerformanceSampleBuffer(2);
            buffer.Add(new PerformanceSample { frameMilliseconds = 10f }); buffer.Add(new PerformanceSample { frameMilliseconds = 20f }); buffer.Add(new PerformanceSample { frameMilliseconds = 30f });
            Assert.That(buffer.Count, Is.EqualTo(2)); Assert.That(buffer.Latest.frameMilliseconds, Is.EqualTo(30f));
        }
    }
}
