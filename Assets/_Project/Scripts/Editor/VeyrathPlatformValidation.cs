using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Veyrath.Editor
{
    /// <summary>Editor-only release checks; never mutates scenes or project settings.</summary>
    public static class VeyrathPlatformValidation
    {
        private const string ReportPath = "Documentation/Reports/02-11-2026 Codex Fase 14 Platformvalidatie/Platform-Validation.md";

        [MenuItem("Veyrath/Validate Platform Readiness")]
        public static void ValidatePlatformReadiness()
        {
            var lines = new List<string>();
            int pass = 0;
            int fail = 0;
            lines.Add("# Platformvalidatie fase 14");
            lines.Add("");
            lines.Add("Automatisch gegenereerd door `Veyrath/Validate Platform Readiness`.");
            lines.Add("");
            Check(lines, ref pass, ref fail, "First Light scene is in build settings", IsSceneEnabled("Assets/_Project/Scenes/FirstLight.unity"));
            Check(lines, ref pass, ref fail, "Ashen Reach scene is in build settings", IsSceneEnabled("Assets/_Project/Scenes/AshenReach.unity"));
            Check(lines, ref pass, ref fail, "macOS build target is installed", BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX));
            Check(lines, ref pass, ref fail, "Windows build target is installed", BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64));
            lines.Add("");
            lines.Add("## Resultaat");
            lines.Add("");
            lines.Add("- Passed: " + pass);
            lines.Add("- Failed: " + fail);
            lines.Add("- macOS runtime build: run separately on the CI/development host.");
            lines.Add("- Windows runtime build: not executed on this macOS host.");
            string directory = Path.GetDirectoryName(ReportPath);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            File.WriteAllLines(ReportPath, lines);
            AssetDatabase.Refresh();
            Debug.Log("Veyrath platform validation written to " + ReportPath + " (" + pass + " passed, " + fail + " failed).");
        }

        [MenuItem("Veyrath/Build macOS Validation Player")]
        public static void BuildMacOSValidationPlayer()
        {
            const string output = "Builds/Phase14-macOS/ProjectVeyrath.app";
            string directory = Path.GetDirectoryName(output);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = GetEnabledScenes(),
                locationPathName = output,
                target = BuildTarget.StandaloneOSX,
                options = BuildOptions.None
            };
            BuildReport report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
                throw new System.Exception("macOS validation build failed: " + report.summary.result);
            Debug.Log("macOS validation player built at " + output);
        }

        private static string[] GetEnabledScenes()
        {
            var scenes = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
                if (scene.enabled) scenes.Add(scene.path);
            return scenes.ToArray();
        }

        private static bool IsSceneEnabled(string path)
        {
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
                if (scene.enabled && scene.path == path) return true;
            return false;
        }

        private static void Check(List<string> lines, ref int pass, ref int fail, string label, bool result)
        {
            lines.Add("- [" + (result ? "x" : " ") + "] " + label);
            if (result) pass++; else fail++;
        }
    }
}
