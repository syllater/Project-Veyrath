using NUnit.Framework;
using Veyrath.Core;

namespace Veyrath.Tests
{
    public sealed class FactionEconomyTests
    {
        private static FactionWorldSimulation CreateWorld()
        {
            var world = new FactionWorldSimulation();
            world.RegisterFaction(new FactionDefinition { id = "wardens", displayName = "First Light Wardens" });
            world.RegisterFaction(new FactionDefinition { id = "ashen", displayName = "Ashen Clans" });
            world.SetTerritory("first-light", "wardens");
            return world;
        }

        [Test]
        public void PlayerActionChangesFactionAndWorldReaction()
        {
            var world = CreateWorld();
            Assert.That(world.ApplyPlayerAction("defend", "wardens", "first-light"), Is.True);
            Assert.That(world.GetFaction("wardens").reputation, Is.EqualTo(10));
            Assert.That(world.GetFaction("wardens").influence, Is.EqualTo(5));
            Assert.That(world.Reactions.Count, Is.EqualTo(1));
        }

        [Test]
        public void TradeRouteMovesRegionalStockAndFundsOwner()
        {
            var world = CreateWorld();
            world.AddStock("first-light", "ore", 10);
            world.AddTradeRoute(new TradeRouteState { id = "ore-road", ownerFactionId = "wardens", originRegionId = "first-light", destinationRegionId = "ashen-reach", resourceId = "ore", capacityPerDay = 4 });
            world.AdvanceDay();
            Assert.That(world.Stock("first-light", "ore"), Is.EqualTo(6));
            Assert.That(world.Stock("ashen-reach", "ore"), Is.EqualTo(4));
            Assert.That(world.GetFaction("wardens").treasury, Is.EqualTo(4));
        }

        [Test]
        public void TreatyBlocksConflictUntilItExpires()
        {
            var world = CreateWorld();
            Assert.That(world.SignTreaty(new TreatyState { id = "pact", factionA = "wardens", factionB = "ashen", type = TreatyType.NonAggression, expiresDay = 2 }), Is.True);
            Assert.That(world.DeclareConflict("war", "wardens", "ashen", "ashen-reach"), Is.False);
            world.AdvanceDay(); world.AdvanceDay(); world.AdvanceDay();
            Assert.That(world.DeclareConflict("war", "wardens", "ashen", "ashen-reach"), Is.True);
        }

        [Test]
        public void TerritoryOwnershipIsReplacedSafely()
        {
            var world = CreateWorld();
            world.SetTerritory("first-light", "ashen");
            Assert.That(world.TerritoryOwner("first-light"), Is.EqualTo("ashen"));
            Assert.That(world.Territories.Count, Is.EqualTo(1));
        }

        [Test]
        public void RiskyRouteCanLoseShipmentWithoutNegativeStock()
        {
            var world = CreateWorld();
            world.AddStock("first-light", "ore", 3);
            world.AddTradeRoute(new TradeRouteState { id = "danger", ownerFactionId = "wardens", originRegionId = "first-light", destinationRegionId = "ashen-reach", resourceId = "ore", capacityPerDay = 3, riskPercent = 100 });
            world.AdvanceDay();
            Assert.That(world.Stock("first-light", "ore"), Is.EqualTo(3));
            Assert.That(world.Stock("ashen-reach", "ore"), Is.Zero);
        }
    }
}
