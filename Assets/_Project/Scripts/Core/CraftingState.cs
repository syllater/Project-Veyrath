namespace Veyrath.Core
{
    public static class CraftingState
    {
        public const string HeartstoneShard = "Heartstone shard";
        public const string WardingCharm = "Warding charm";

        public static bool CraftWardingCharm(InventoryState inventory)
        {
            if (inventory == null || !inventory.Contains(HeartstoneShard) || inventory.Contains(WardingCharm)) return false;
            inventory.RemoveItem(HeartstoneShard);
            inventory.AddItem(WardingCharm);
            return true;
        }
    }
}
