using NUnit.Framework;
using Veyrath.Core;

namespace Veyrath.Tests
{
    public sealed class SaveDataTests
    {
        [Test]
        public void CurrentSchemaIsSupported()
        {
            SaveData data = new SaveData();

            Assert.That(data.schemaVersion, Is.EqualTo(SaveData.CurrentSchemaVersion));
            Assert.That(data.IsSupportedSchema(), Is.True);
        }

        [Test]
        public void PreviousSchemaCanBeMigratedInMemory()
        {
            SaveData data = new SaveData { schemaVersion = 1 };

            Assert.That(data.IsSupportedSchema(), Is.True);
            data.schemaVersion = SaveData.CurrentSchemaVersion;
            Assert.That(data.schemaVersion, Is.EqualTo(2));
        }
    }
}
