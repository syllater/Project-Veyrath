#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Veyrath.Editor
{
    /// <summary>Applies the checked-in foundation in one repeatable Editor action.</summary>
    public static class VeyrathProjectSetup
    {
        [MenuItem("Veyrath/Configure Project Foundation")]
        public static void Configure()
        {
            ConfigureSerializationAndInput();
            ConfigurePlayerDefaults();
            AssetDatabase.SaveAssets();
            Veyrath.Core.VeyrathLogger.Info("Setup", "Project foundation configured with keyboard input compatibility and desktop defaults. The render pipeline remains at the Unity project default.");
        }

        private static void ConfigureSerializationAndInput()
        {
            EditorSettings.serializationMode = SerializationMode.ForceText;
            // This slice currently uses UnityEngine.Input. Both keeps that API active while allowing
            // the project to adopt the Input System package later without replacing the editor setting.
            PlayerSettings.SetPropertyInt("activeInputHandler", 2, BuildTargetGroup.Standalone);
        }

        private static void ConfigurePlayerDefaults()
        {
            PlayerSettings.productName = "Project Veyrath";
            PlayerSettings.defaultScreenWidth = 1280;
            PlayerSettings.defaultScreenHeight = 720;
            PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
            PlayerSettings.resizableWindow = true;
            PlayerSettings.runInBackground = true;
        }
    }
}
#endif
