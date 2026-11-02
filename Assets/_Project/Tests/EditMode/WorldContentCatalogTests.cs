using NUnit.Framework;
using Veyrath.Core;
using System.Collections.Generic;

namespace Veyrath.Tests
{
    public sealed class WorldContentCatalogTests
    {
        [Test]
        public void WorldContainsEightAuthoredRegionsWithContent()
        {
            Assert.That(RegionCatalog.All.Count, Is.EqualTo(8));
            foreach (RegionDefinition region in RegionCatalog.All)
            {
                Assert.That(region.content, Is.Not.Null);
                Assert.That(region.content.settlements.Length, Is.GreaterThan(0));
                Assert.That(region.content.bosses.Length, Is.GreaterThan(0));
                Assert.That(region.content.worldEvents.Length, Is.GreaterThan(0));
            }
        }

        [Test]
        public void AllRegionsAreReachableFromFirstLight()
        {
            var visited = new HashSet<string>(); var pending = new Queue<string>(); pending.Enqueue("first-light");
            while (pending.Count > 0)
            {
                string id = pending.Dequeue(); if (!visited.Add(id)) continue;
                RegionDefinition region = RegionCatalog.Get(id); if (region == null) continue;
                foreach (string next in region.connectedRegionIds) pending.Enqueue(next);
            }
            Assert.That(visited.Count, Is.EqualTo(8));
        }

        [Test]
        public void CatalogProvidesDistinctContentIdsAcrossRegions()
        {
            var bosses = new HashSet<string>();
            foreach (RegionDefinition region in RegionCatalog.All) foreach (string boss in region.content.bosses) Assert.That(bosses.Add(boss), Is.True);
        }
    }
}
