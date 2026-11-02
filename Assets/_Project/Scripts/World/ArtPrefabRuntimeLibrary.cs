using UnityEngine;

namespace Veyrath.World
{
    /// <summary>Loads authored art prefabs when available and keeps the prototype's primitive fallback intact.</summary>
    public static class ArtPrefabRuntimeLibrary
    {
        public static GameObject Spawn(string resourcePath, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            GameObject prefab = Resources.Load<GameObject>(resourcePath);
            if (prefab == null) return null;
            GameObject instance = Object.Instantiate(prefab, position, rotation);
            instance.transform.localScale = scale;
            return instance;
        }

        public static GameObject Spawn(string resourcePath, Vector3 position, Vector3 scale)
        {
            return Spawn(resourcePath, position, Quaternion.identity, scale);
        }
    }
}
