using System;

namespace Veyrath.Core
{
    [Serializable]
    public sealed class SaveData
    {
        public const int CurrentSchemaVersion = 3;
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
        public int level = 1;
        public int experience;
        public int unspentSkillPoints;
        public string[] skills = Array.Empty<string>();
        public string equippedWeapon;
        public int eddaFactionReputation;

        public bool IsSupportedSchema()
        {
            return schemaVersion >= 1 && schemaVersion <= CurrentSchemaVersion;
        }
    }
}
