using NUnit.Framework;
using Veyrath.Core;

namespace Veyrath.Tests
{
    public sealed class CoreSystemsTests
    {
        [Test]
        public void ExperienceLevelsCharacterAndGrantsSkillPoint()
        {
            CharacterProgression progression = new CharacterProgression();

            progression.AddExperience(100);

            Assert.That(progression.level, Is.EqualTo(2));
            Assert.That(progression.unspentSkillPoints, Is.EqualTo(1));
        }

        [Test]
        public void SkillUnlockConsumesPointAndRaisesRank()
        {
            CharacterProgression progression = new CharacterProgression { unspentSkillPoints = 1 };
            SkillState skills = new SkillState();

            Assert.That(skills.Unlock("Swordsmanship", progression), Is.True);
            Assert.That(skills.Rank("Swordsmanship"), Is.EqualTo(1));
            Assert.That(progression.unspentSkillPoints, Is.Zero);
        }

        [Test]
        public void EquipmentRequiresOwnedItem()
        {
            InventoryState inventory = new InventoryState();
            EquipmentState equipment = new EquipmentState();

            Assert.That(equipment.EquipWeapon("Ashen shortblade", inventory), Is.False);
            inventory.AddItem("Ashen shortblade");
            Assert.That(equipment.EquipWeapon("Ashen shortblade", inventory), Is.True);
        }

        [Test]
        public void CraftingConsumesShardAndCreatesCharm()
        {
            InventoryState inventory = new InventoryState();
            inventory.AddItem(CraftingState.HeartstoneShard);

            Assert.That(CraftingState.CraftWardingCharm(inventory), Is.True);
            Assert.That(inventory.Contains(CraftingState.HeartstoneShard), Is.False);
            Assert.That(inventory.Contains(CraftingState.WardingCharm), Is.True);
        }

        [Test]
        public void ReputationIsClamped()
        {
            FactionReputation reputation = new FactionReputation();

            reputation.Adjust(200);

            Assert.That(reputation.eddaFaction, Is.EqualTo(100));
        }
    }
}
