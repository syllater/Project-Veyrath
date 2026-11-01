using System;

namespace Veyrath.Core
{
    [Serializable]
    public sealed class EquipmentState
    {
        public string weaponId;

        public bool EquipWeapon(string id, InventoryState inventory)
        {
            if (string.IsNullOrWhiteSpace(id) || inventory == null || !inventory.Contains(id)) return false;
            weaponId = id;
            return true;
        }

        public bool HasWeapon(string id)
        {
            return !string.IsNullOrWhiteSpace(id) && weaponId == id;
        }
    }
}
