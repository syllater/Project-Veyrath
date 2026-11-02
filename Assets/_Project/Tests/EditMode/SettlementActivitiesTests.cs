using NUnit.Framework;
using Veyrath.Core;

namespace Veyrath.Tests
{
    public sealed class SettlementActivitiesTests
    {
        private static SettlementState CreateSettlement()
        {
            return new SettlementState { id = "camp", regionId = "first-light", residents = 2, resources = new System.Collections.Generic.List<SettlementResource>() };
        }

        [Test]
        public void GatheringAndBuildingFormSettlementProgressionLoop()
        {
            var settlement = CreateSettlement(); var service = new SettlementActivityService();
            service.Execute(settlement, SettlementActivity.Gathering);
            Assert.That(service.Build(settlement, "fishery", 2, 3), Is.True);
            Assert.That(settlement.HasBuilding("fishery"), Is.True);
            Assert.That(settlement.defense, Is.EqualTo(3));
        }

        [Test]
        public void FishingCookingAndDailyConsumptionFormFoodLoop()
        {
            var settlement = CreateSettlement(); settlement.buildings.Add("fishery"); var service = new SettlementActivityService();
            Assert.That(service.Execute(settlement, SettlementActivity.Fishing).success, Is.True);
            Assert.That(service.Execute(settlement, SettlementActivity.Cooking).success, Is.True);
            int before = settlement.ResourceAmount("meal"); service.AdvanceDay(settlement);
            Assert.That(before, Is.EqualTo(1)); Assert.That(settlement.ResourceAmount("meal"), Is.Zero); Assert.That(settlement.morale, Is.GreaterThan(0));
        }

        [Test]
        public void HuntingProducesIngredientsForCooking()
        {
            var settlement = CreateSettlement(); settlement.buildings.Add("hunting-lodge"); var service = new SettlementActivityService();
            Assert.That(service.Execute(settlement, SettlementActivity.Hunting).success, Is.True);
            Assert.That(service.Execute(settlement, SettlementActivity.Cooking).outputId, Is.EqualTo("meal"));
            Assert.That(settlement.ResourceAmount("hide"), Is.EqualTo(1));
        }

        [Test]
        public void GatheringAndAlchemyConsumeHerbsIntoPotion()
        {
            var settlement = CreateSettlement(); var service = new SettlementActivityService();
            service.Execute(settlement, SettlementActivity.Gathering); service.Execute(settlement, SettlementActivity.Gathering);
            Assert.That(service.Execute(settlement, SettlementActivity.Alchemy).success, Is.True);
            Assert.That(settlement.ResourceAmount("healing-potion"), Is.EqualTo(1));
        }

        [Test]
        public void RidingRequiresStableAndMountAndAdvancesDistance()
        {
            var settlement = CreateSettlement(); settlement.mountId = "bramblehorse"; var service = new SettlementActivityService();
            Assert.That(service.Execute(settlement, SettlementActivity.Riding).success, Is.False);
            settlement.buildings.Add("stable");
            Assert.That(service.Execute(settlement, SettlementActivity.Riding).success, Is.True);
            Assert.That(settlement.travelDistance, Is.EqualTo(10));
        }

        [Test]
        public void ActivityRequirementsPreventFreeOutputs()
        {
            var settlement = CreateSettlement(); var service = new SettlementActivityService();
            Assert.That(service.Execute(settlement, SettlementActivity.Fishing).success, Is.False);
            Assert.That(service.Execute(settlement, SettlementActivity.Alchemy).success, Is.False);
            Assert.That(settlement.ResourceAmount("fish"), Is.Zero);
        }
    }
}
