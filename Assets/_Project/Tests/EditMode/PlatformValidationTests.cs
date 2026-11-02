using System.IO;
using NUnit.Framework;
using Veyrath.Core;

namespace Veyrath.Tests
{
    public sealed class PlatformValidationTests
    {
        [Test]
        public void SavePath_IsInsidePersistentDataDirectory()
        {
            string path = PlatformValidation.SavePath();
            Assert.That(path, Does.StartWith(PlatformValidation.SaveDirectory));
            Assert.That(Path.GetFileName(path), Is.EqualTo("veyrath-first-light.json"));
        }

        [Test]
        public void EmptyFileName_UsesSafeDefault()
        {
            Assert.That(Path.GetFileName(PlatformValidation.SavePath(string.Empty)), Is.EqualTo("veyrath-first-light.json"));
        }

        [Test]
        public void Snapshot_ContainsPlatformAndSaveLocation()
        {
            PlatformValidationSnapshot snapshot = PlatformValidation.Capture();
            Assert.That(snapshot.platform, Is.Not.Empty);
            Assert.That(snapshot.persistentDataPath, Is.EqualTo(PlatformValidation.SaveDirectory));
        }

        [Test]
        public void SaveDirectory_IsAvailableForTests()
        {
            Assert.That(PlatformValidation.HasPersistentSaveDirectory(), Is.True);
        }
    }
}
