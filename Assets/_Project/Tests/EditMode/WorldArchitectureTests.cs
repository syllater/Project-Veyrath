using NUnit.Framework;
using Veyrath.Core;

namespace Veyrath.Tests
{
    public sealed class WorldArchitectureTests
    {
        [Test]
        public void ConnectedRegionsTransitionWithoutLosingDiscoveries()
        {
            WorldState world = new WorldState();
            world.Discover("first-light-clearing");
            world.UnlockAnchor("first-light-camp", "first-light");

            Assert.That(world.EnterRegion("ashen-reach"), Is.True);
            world.Discover("ashen-reach-gate");
            Assert.That(world.EnterRegion("first-light"), Is.True);
            Assert.That(world.discoveries.Exists(item => item.id == "ashen-reach-gate"), Is.True);
            Assert.That(world.worldMinutes, Is.EqualTo(30));
        }

        [Test]
        public void UnconnectedRegionIsRejected()
        {
            WorldState world = new WorldState();

            Assert.That(world.EnterRegion("unknown-region"), Is.False);
            Assert.That(world.currentRegionId, Is.EqualTo("first-light"));
        }

        [Test]
        public void ClockAndWeatherRemainBoundedAndConfigurable()
        {
            WorldState world = new WorldState();

            world.AdvanceTime(24 * 60 + 45);
            world.SetWeather("mist");

            Assert.That(world.worldMinutes, Is.EqualTo(45));
            Assert.That(world.weather, Is.EqualTo("mist"));
        }

        [Test]
        public void ReEnteringCurrentRegionIsIdempotent()
        {
            WorldState world = new WorldState();
            world.AdvanceTime(20);
            Assert.That(world.EnterRegion("first-light"), Is.True);
            Assert.That(world.worldMinutes, Is.EqualTo(20));
        }
    }
}
