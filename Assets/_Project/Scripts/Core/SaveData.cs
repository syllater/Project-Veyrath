using System;

namespace Veyrath.Core
{
    [Serializable]
    public sealed class SaveData
    {
        public int schemaVersion = 1;
        public float playerX;
        public float playerY;
        public float playerZ;
        public float health = 100f;
        public FirstLightQuestStage questStage;
        public bool rewardClaimed;
        public int coins;
        public string[] items = Array.Empty<string>();
        public bool wraithDefeated;
    }
}
