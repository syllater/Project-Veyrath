using System;
using System.Collections.Generic;

namespace Veyrath.Core
{
    public enum VeyrathLanguage { Dutch, English }

    [Serializable]
    public sealed class LocalizedText
    {
        public string key;
        public string dutch;
        public string english;
    }

    public sealed class LocalizationService
    {
        public VeyrathLanguage Language { get; set; } = VeyrathLanguage.Dutch;
        public readonly List<LocalizedText> Entries = new List<LocalizedText>();

        public string Get(string key)
        {
            LocalizedText entry = Entries.Find(item => item.key == key);
            if (entry == null) return key ?? string.Empty;
            string value = Language == VeyrathLanguage.Dutch ? entry.dutch : entry.english;
            return string.IsNullOrWhiteSpace(value) ? (Language == VeyrathLanguage.Dutch ? entry.english : entry.dutch) : value;
        }

        public bool Validate(out List<string> missingKeys)
        {
            missingKeys = new List<string>();
            foreach (LocalizedText entry in Entries)
                if (entry == null || string.IsNullOrWhiteSpace(entry.key) || string.IsNullOrWhiteSpace(entry.dutch) || string.IsNullOrWhiteSpace(entry.english))
                    missingKeys.Add(entry == null ? "<null>" : entry.key);
            return missingKeys.Count == 0;
        }
    }

    [Serializable]
    public sealed class SubtitleLine
    {
        public string speaker;
        public string textKey;
        public float durationSeconds = 3f;
    }

    public sealed class SubtitleService
    {
        public SubtitleLine Current { get; private set; }
        public float RemainingSeconds { get; private set; }

        public void Show(SubtitleLine line)
        {
            Current = line;
            RemainingSeconds = line == null ? 0f : Math.Max(.1f, line.durationSeconds);
        }

        public void Tick(float seconds)
        {
            if (Current == null || seconds <= 0f) return;
            RemainingSeconds = Math.Max(0f, RemainingSeconds - seconds);
            if (RemainingSeconds <= 0f) Current = null;
        }
    }

    [Serializable]
    public sealed class TtsAssetRecord
    {
        public string key;
        public string locale;
        public string relativePath;
        public bool required;
    }

    public sealed class TtsProductionManifest
    {
        public readonly List<TtsAssetRecord> Assets = new List<TtsAssetRecord>();

        public bool Validate(Func<string, bool> exists, out List<string> missing)
        {
            missing = new List<string>();
            foreach (TtsAssetRecord asset in Assets)
                if (asset != null && asset.required && (exists == null || !exists(asset.relativePath))) missing.Add(asset.key + " (" + asset.locale + ")");
            return missing.Count == 0;
        }
    }
}
