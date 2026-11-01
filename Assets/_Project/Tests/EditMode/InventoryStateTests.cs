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
    }
}
