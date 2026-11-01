using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Veyrath.Core;
using Veyrath.World;

namespace Veyrath.Tests
{
    public sealed class FirstLightPlayModeTests
    {
        [UnityTest]
        public IEnumerator FirstLightBuildsItsRuntimeWorld()
        {
            SceneManager.LoadScene("FirstLight");
            yield return null;
            yield return null;

            Assert.That(GameObject.Find("Player | Placeholder adventurer"), Is.Not.Null);
            Assert.That(GameObject.Find("Edda | NPC placeholder"), Is.Not.Null);
            Assert.That(GameObject.Find("Wraith | Enemy placeholder"), Is.Not.Null);
            Assert.That(GameObject.Find("First Light Camera"), Is.Not.Null);
            Assert.That(FindIncludingInactive("Loot | Wraith heartstone"), Is.Not.Null);
        }

        private static GameObject FindIncludingInactive(string name)
        {
            foreach (GameObject candidate in Resources.FindObjectsOfTypeAll<GameObject>())
                if (candidate.name == name) return candidate;
            return null;
        }

        [UnityTest]
        public IEnumerator FirstLightCompletesGameplayAndCoreSystemsFlow()
        {
            SceneManager.LoadScene("FirstLight");
            yield return null;
            yield return null;
            VeyrathGameController controller = Object.FindFirstObjectByType<VeyrathGameController>();
            Component playerComponent = (Component)GetField(controller, "player").GetValue(controller);
            Transform player = playerComponent.transform;
            Transform npc = (Transform)GetField(controller, "npc").GetValue(controller);
            Transform wraith = (Transform)GetField(controller, "wraith").GetValue(controller);
            InventoryState inventory = (InventoryState)GetField(controller, "inventory").GetValue(controller);
            QuestState quest = (QuestState)GetField(controller, "quest").GetValue(controller);
            CharacterProgression progression = (CharacterProgression)GetField(controller, "progression").GetValue(controller);
            EquipmentState equipment = (EquipmentState)GetField(controller, "equipment").GetValue(controller);
            FactionReputation reputation = (FactionReputation)GetField(controller, "reputation").GetValue(controller);

            player.position = npc.position;
            Invoke(controller, "Interact");
            Assert.That(quest.stage, Is.EqualTo(FirstLightQuestStage.HuntTheWraith));
            Assert.That(equipment.HasWeapon("Ashen shortblade"), Is.True);

            player.position = wraith.position;
            for (int i = 0; i < 3; i++)
            {
                GetField(controller, "attackCooldown").SetValue(controller, 0f);
                Invoke(controller, "Attack");
            }
            Assert.That(quest.stage, Is.EqualTo(FirstLightQuestStage.ReturnToEdda));
            Assert.That(progression.level, Is.EqualTo(2));

            GameObject loot = FindIncludingInactive("Loot | Wraith heartstone");
            player.position = loot.transform.position;
            Invoke(controller, "CollectLoot");
            Assert.That(inventory.Contains(CraftingState.HeartstoneShard), Is.True);

            player.position = npc.position;
            Invoke(controller, "CraftAtEdda");
            Invoke(controller, "Interact");
            Assert.That(quest.stage, Is.EqualTo(FirstLightQuestStage.Complete));
            Assert.That(inventory.Contains(CraftingState.WardingCharm), Is.True);
            Assert.That(reputation.eddaFaction, Is.GreaterThan(0));

            Invoke(controller, "Save");
            Invoke(controller, "Load");
            Assert.That(equipment.HasWeapon("Ashen shortblade"), Is.True);
            Assert.That(inventory.Contains("Edda's iron charm"), Is.True);
        }

        private static FieldInfo GetField(object instance, string name)
        {
            return instance.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
        }

        private static void Invoke(object instance, string name)
        {
            instance.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(instance, null);
        }
    }
}
