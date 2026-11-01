using System;
using System.Collections.Generic;

namespace Veyrath.Core
{
    [Serializable]
    public sealed class InventoryState
    {
        public int coins;
        public List<string> items = new List<string>();

        public void AddReward(string item, int coinAmount)
        {
            if (!string.IsNullOrWhiteSpace(item)) items.Add(item);
            coins = Math.Max(0, coins + coinAmount);
        }
    }
}
