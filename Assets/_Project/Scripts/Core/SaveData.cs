using System;

namespace Veyrath.Core
{
    [Serializable]
    public sealed class SaveData
    {
        public const int CurrentSchemaVersion = 4;
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
        public string currentRegionId = "first-light";
        public int worldMinutes;
        public string weather = "clear";
        public string[] discoveries = Array.Empty<string>();
        public string[] travelAnchors = Array.Empty<string>();
        public string npcStates = string.Empty;

        public bool IsSupportedSchema()
        {
            return schemaVersion >= 1 && schemaVersion <= CurrentSchemaVersion;
        }
    }
}
