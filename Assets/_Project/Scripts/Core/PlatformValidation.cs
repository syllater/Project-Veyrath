using System;
using System.IO;
using UnityEngine;

namespace Veyrath.Core
{
    /// <summary>Small platform-neutral diagnostics surface used by startup checks and release QA.</summary>
    public static class PlatformValidation
    {
        public static string SaveDirectory { get { return Application.persistentDataPath; } }
        public static string SavePath(string fileName = "veyrath-first-light.json")
        {
            if (string.IsNullOrEmpty(fileName)) fileName = "veyrath-first-light.json";
            return Path.Combine(SaveDirectory, fileName);
        }

        public static bool HasPersistentSaveDirectory()
        {
            try { return !string.IsNullOrEmpty(SaveDirectory) && Directory.Exists(SaveDirectory); }
            catch (Exception) { return false; }
        }

        public static PlatformValidationSnapshot Capture()
        {
            return new PlatformValidationSnapshot
            {
                platform = Application.platform.ToString(),
                graphicsDevice = SystemInfo.graphicsDeviceName,
                graphicsApi = SystemInfo.graphicsDeviceType.ToString(),
                persistentDataPath = SaveDirectory,
                saveDirectoryAvailable = HasPersistentSaveDirectory(),
                screenWidth = Screen.width,
                screenHeight = Screen.height,
                targetFrameRate = Application.targetFrameRate,
                masterVolume = AudioListener.volume,
                scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            };
        }
    }

    [Serializable]
    public struct PlatformValidationSnapshot
    {
        public string platform;
        public string graphicsDevice;
        public string graphicsApi;
        public string persistentDataPath;
        public bool saveDirectoryAvailable;
        public int screenWidth;
        public int screenHeight;
        public int targetFrameRate;
        public float masterVolume;
        public string scene;

        public bool IsUsable()
        {
            return !string.IsNullOrEmpty(platform)
                && !string.IsNullOrEmpty(persistentDataPath)
                && saveDirectoryAvailable;
        }
    }
}
