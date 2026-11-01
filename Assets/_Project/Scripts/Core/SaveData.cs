using System;

namespace Veyrath.Core
{
    [Serializable]
    public sealed class SaveData
    {
        public const int CurrentSchemaVersion = 2;
        public int schemaVersion = CurrentSchemaVersion;
        public float playerX;
        public float playerY;
        public float playerZ;
        public float health = 100f;
        public FirstLightQuestStage questStage;
        public bool rewardClaimed;
        public int coins;
        public string[] items = Array.Empty<string>();
        public bool wraithDefeated;
        public bool lootCollected;

        public bool IsSupportedSchema()
        {
            return schemaVersion == 1 || schemaVersion == CurrentSchemaVersion;
        }
    }
}
