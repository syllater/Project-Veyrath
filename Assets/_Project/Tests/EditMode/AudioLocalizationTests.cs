using NUnit.Framework;
using Veyrath.Core;

namespace Veyrath.Tests
{
    public sealed class AudioLocalizationTests
    {
        [Test]
        public void LocalizationReturnsSelectedLanguageAndSafeFallback()
        {
            var service = new LocalizationService();
            service.Entries.Add(new LocalizedText { key = "greet", dutch = "Hallo", english = "Hello" });
            service.Language = VeyrathLanguage.English;
            Assert.That(service.Get("greet"), Is.EqualTo("Hello"));
            Assert.That(service.Get("missing"), Is.EqualTo("missing"));
        }

        [Test]
        public void LocalizationValidationFindsIncompleteEntries()
        {
            var service = new LocalizationService(); service.Entries.Add(new LocalizedText { key = "broken", dutch = "Kapotte tekst" });
            Assert.That(service.Validate(out var missing), Is.False); Assert.That(missing, Does.Contain("broken"));
        }

        [Test]
        public void SubtitleLinesExpireWithoutBlockingDialogue()
        {
            var subtitles = new SubtitleService(); subtitles.Show(new SubtitleLine { speaker = "Edda", textKey = "greet", durationSeconds = 1f });
            subtitles.Tick(.5f); Assert.That(subtitles.Current, Is.Not.Null);
            subtitles.Tick(.6f); Assert.That(subtitles.Current, Is.Null);
        }

        [Test]
        public void TtsManifestReportsMissingFilesWithoutThrowing()
        {
            var manifest = new TtsProductionManifest();
            manifest.Assets.Add(new TtsAssetRecord { key = "edda.greet", locale = "nl-NL", relativePath = "Audio/edda-greet.wav", required = true });
            Assert.That(manifest.Validate(path => false, out var missing), Is.False);
            Assert.That(missing.Count, Is.EqualTo(1));
        }

        [Test]
        public void TtsManifestPassesWhenRequiredFilesExist()
        {
            var manifest = new TtsProductionManifest();
            manifest.Assets.Add(new TtsAssetRecord { key = "edda.greet", locale = "en-US", relativePath = "Audio/edda-greet.wav", required = true });
            Assert.That(manifest.Validate(path => path.EndsWith(".wav"), out _), Is.True);
        }
    }
}
