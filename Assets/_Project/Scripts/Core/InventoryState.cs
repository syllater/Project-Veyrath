using System;
using System.Collections.Generic;

namespace Veyrath.Core
{
    [Serializable]
    public sealed class InventoryState
    {
        public int coins;
        public List<string> items = new List<string>();

        public bool Contains(string item)
        {
            return !string.IsNullOrWhiteSpace(item) && items.Contains(item);
        }

        public void AddItem(string item)
        {
            if (!string.IsNullOrWhiteSpace(item) && !Contains(item)) items.Add(item);
        }

        public bool RemoveItem(string item)
        {
            return !string.IsNullOrWhiteSpace(item) && items.Remove(item);
        }

        public void AddReward(string item, int coinAmount)
        {
            AddItem(item);
            coins = Math.Max(0, coins + coinAmount);
        }
    }
}
