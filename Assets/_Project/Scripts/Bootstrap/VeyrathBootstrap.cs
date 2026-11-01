using UnityEngine;
using Veyrath.Core;
using Veyrath.World;

namespace Veyrath.Bootstrap
{
    /// <summary>Applies saved options and supplies the game controller if a build opens without one.</summary>
    public static class VeyrathBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            VeyrathSettings.Initialize();
            if (Object.FindFirstObjectByType<VeyrathGameController>() == null)
            {
                GameObject root = new GameObject("Veyrath Game Controller (Bootstrap)");
                root.AddComponent<VeyrathGameController>();
                VeyrathLogger.Warning("Bootstrap", "No scene controller was found; created a runtime fallback.");
            }
            else
            {
                VeyrathLogger.Info("Bootstrap", "First Light scene is ready.");
            }
        }
    }
}
