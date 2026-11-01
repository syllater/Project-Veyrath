#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Veyrath.World;

namespace Veyrath.Editor
{
    public static class FirstLightSceneBuilder
    {
        [MenuItem("Veyrath/Generate First Light Scene")]
        public static void Build()
        {
            VeyrathProjectSetup.Configure();
            const string folder = "Assets/_Project/Scenes";
            Directory.CreateDirectory(folder);
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            GameObject controller = new GameObject("Veyrath Game Controller");
            controller.AddComponent<VeyrathGameController>();
            EditorSceneManager.SaveScene(scene, folder + "/FirstLight.unity");
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(folder + "/FirstLight.unity", true) };
            AssetDatabase.SaveAssets();
            Debug.Log("Generated First Light scene and set it as the first build scene.");
        }
    }
}
#endif
