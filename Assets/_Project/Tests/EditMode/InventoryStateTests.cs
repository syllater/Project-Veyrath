using NUnit.Framework;
using Veyrath.Core;

namespace Veyrath.Tests
{
    public sealed class InventoryStateTests
    {
        [Test]
        public void RewardAddsItemAndCurrency()
        {
            InventoryState inventory = new InventoryState();

            inventory.AddReward("Edda's iron charm", 25);

            Assert.That(inventory.items, Does.Contain("Edda's iron charm"));
            Assert.That(inventory.coins, Is.EqualTo(25));
        }

        [Test]
        public void EmptyItemIsIgnoredAndNegativeCoinAmountCannotCreateDebt()
        {
            InventoryState inventory = new InventoryState();

            inventory.AddReward("", -25);

            Assert.That(inventory.items, Is.Empty);
            Assert.That(inventory.coins, Is.Zero);
        }

        [Test]
        public void NamedLootAndWeaponAreUniqueInventoryEntries()
        {
            InventoryState inventory = new InventoryState();

            inventory.AddItem("Ashen shortblade");
            inventory.AddItem("Wraith heartstone");
            inventory.AddItem("Wraith heartstone");

            Assert.That(inventory.Contains("Ashen shortblade"), Is.True);
            Assert.That(inventory.items.Count, Is.EqualTo(2));
        }
    }
}
