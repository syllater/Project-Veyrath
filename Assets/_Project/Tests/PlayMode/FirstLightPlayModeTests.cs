using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

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
    }
}
