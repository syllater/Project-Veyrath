using UnityEngine;

namespace Veyrath.Core
{
    /// <summary>Small persistent settings facade for the prototype's actual Unity runtime options.</summary>
    public static class VeyrathSettings
    {
        private const string FullscreenKey = "veyrath.settings.fullscreen";
        private const string VSyncKey = "veyrath.settings.vsync";
        private const string FrameRateKey = "veyrath.settings.framerate";
        private const string QualityKey = "veyrath.settings.quality";
        private const string VolumeKey = "veyrath.settings.volume";
        private static readonly int[] FrameRates = { 30, 60, 120, -1 };
        private static bool initialized;

        public static void Initialize()
        {
            if (initialized) return;
            initialized = true;
            Screen.fullScreen = PlayerPrefs.GetInt(FullscreenKey, Screen.fullScreen ? 1 : 0) == 1;
            QualitySettings.vSyncCount = PlayerPrefs.GetInt(VSyncKey, 0);
            Application.targetFrameRate = PlayerPrefs.GetInt(FrameRateKey, 60);
            int quality = Mathf.Clamp(PlayerPrefs.GetInt(QualityKey, QualitySettings.GetQualityLevel()), 0, QualitySettings.names.Length - 1);
            QualitySettings.SetQualityLevel(quality, true);
            AudioListener.volume = Mathf.Clamp01(PlayerPrefs.GetFloat(VolumeKey, 1f));
        }

        public static bool IsFullscreen { get { return Screen.fullScreen; } }
        public static bool IsVSync { get { return QualitySettings.vSyncCount > 0; } }
        public static int TargetFrameRate { get { return Application.targetFrameRate; } }
        public static int QualityLevel { get { return QualitySettings.GetQualityLevel(); } }
        public static float MasterVolume { get { return AudioListener.volume; } }

        public static void ToggleFullscreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
            PlayerPrefs.SetInt(FullscreenKey, Screen.fullScreen ? 1 : 0);
            PlayerPrefs.Save();
        }

        public static void ToggleVSync()
        {
            QualitySettings.vSyncCount = IsVSync ? 0 : 1;
            PlayerPrefs.SetInt(VSyncKey, QualitySettings.vSyncCount);
            PlayerPrefs.Save();
        }

        public static void CycleFrameRate()
        {
            int index = 0;
            for (int i = 0; i < FrameRates.Length; i++)
                if (FrameRates[i] == Application.targetFrameRate) index = (i + 1) % FrameRates.Length;
            Application.targetFrameRate = FrameRates[index];
            PlayerPrefs.SetInt(FrameRateKey, Application.targetFrameRate);
            PlayerPrefs.Save();
        }

        public static void CycleQuality()
        {
            int count = QualitySettings.names.Length;
            if (count == 0) return;
            int next = (QualitySettings.GetQualityLevel() + 1) % count;
            QualitySettings.SetQualityLevel(next, true);
            PlayerPrefs.SetInt(QualityKey, next);
            PlayerPrefs.Save();
        }

        public static void CycleResolution()
        {
            Resolution[] resolutions = Screen.resolutions;
            if (resolutions == null || resolutions.Length < 2) return;
            int current = 0;
            for (int i = 0; i < resolutions.Length; i++)
                if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height) current = i;
            Resolution next = resolutions[(current + 1) % resolutions.Length];
            Screen.SetResolution(next.width, next.height, Screen.fullScreen);
        }

        public static void SetMasterVolume(float volume)
        {
            AudioListener.volume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(VolumeKey, AudioListener.volume);
            PlayerPrefs.Save();
        }

        public static string FrameRateLabel
        {
            get { return Application.targetFrameRate < 0 ? "Unlimited" : Application.targetFrameRate + " FPS"; }
        }

        public static string QualityLabel
        {
            get
            {
                string[] names = QualitySettings.names;
                int level = QualitySettings.GetQualityLevel();
                return names.Length > 0 ? names[Mathf.Clamp(level, 0, names.Length - 1)] : "Default";
            }
        }
    }
}
