using System;
using System.Collections.Generic;

namespace Veyrath.Core
{
    public enum SettlementActivity { Gathering, Fishing, Hunting, Cooking, Alchemy, Riding }

    [Serializable]
    public sealed class SettlementResource
    {
        public string id;
        public int amount;
    }

    [Serializable]
    public sealed class SettlementState
    {
        public string id;
        public string regionId;
        public int residents;
        public int defense;
        public int morale = 50;
        public List<string> buildings = new List<string>();
        public List<SettlementResource> resources = new List<SettlementResource>();
        public string mountId = string.Empty;
        public int travelDistance;

        public int ResourceAmount(string resourceId)
        {
            SettlementResource resource = resources.Find(item => item.id == resourceId);
            return resource == null ? 0 : resource.amount;
        }

        public void AddResource(string resourceId, int amount)
        {
            if (string.IsNullOrWhiteSpace(resourceId) || amount == 0) return;
            SettlementResource resource = resources.Find(item => item.id == resourceId);
            if (resource == null) { resource = new SettlementResource { id = resourceId }; resources.Add(resource); }
            resource.amount = Math.Max(0, resource.amount + amount);
        }

        public bool HasBuilding(string buildingId) { return !string.IsNullOrWhiteSpace(buildingId) && buildings.Contains(buildingId); }
    }

    [Serializable]
    public sealed class ActivityResult
    {
        public bool success;
        public string outputId;
        public int outputAmount;
        public string message;
    }

    public sealed class SettlementActivityService
    {
        public ActivityResult Execute(SettlementState settlement, SettlementActivity activity)
        {
            if (settlement == null) return Fail("No settlement supplied.");
            switch (activity)
            {
                case SettlementActivity.Gathering:
                    settlement.AddResource("wood", 3); settlement.AddResource("herb", 1);
                    return Success("wood", 3, "Gathered wood and herbs.");
                case SettlementActivity.Fishing:
                    if (!settlement.HasBuilding("fishery")) return Fail("A fishery is required.");
                    settlement.AddResource("fish", 2); return Success("fish", 2, "Caught fish.");
                case SettlementActivity.Hunting:
                    if (!settlement.HasBuilding("hunting-lodge")) return Fail("A hunting lodge is required.");
                    settlement.AddResource("raw-meat", 2); settlement.AddResource("hide", 1); return Success("raw-meat", 2, "Hunted game.");
                case SettlementActivity.Cooking:
                    if (!Consume(settlement, "raw-meat", 1) && !Consume(settlement, "fish", 1)) return Fail("Raw meat or fish is required.");
                    settlement.AddResource("meal", 1); return Success("meal", 1, "Cooked a meal.");
                case SettlementActivity.Alchemy:
                    if (!Consume(settlement, "herb", 2)) return Fail("Two herbs are required.");
                    settlement.AddResource("healing-potion", 1); return Success("healing-potion", 1, "Brewed a healing potion.");
                case SettlementActivity.Riding:
                    if (string.IsNullOrWhiteSpace(settlement.mountId) || !settlement.HasBuilding("stable")) return Fail("A stable and mount are required.");
                    settlement.travelDistance += 10; return Success("travel", 10, "Travelled by mount.");
                default: return Fail("Unknown activity.");
            }
        }

        public bool Build(SettlementState settlement, string buildingId, int materialCost, int defenseGain)
        {
            if (settlement == null || string.IsNullOrWhiteSpace(buildingId) || settlement.HasBuilding(buildingId) || settlement.ResourceAmount("wood") < materialCost) return false;
            settlement.AddResource("wood", -Math.Max(0, materialCost)); settlement.buildings.Add(buildingId); settlement.defense = Math.Max(0, settlement.defense + Math.Max(0, defenseGain)); return true;
        }

        public void AdvanceDay(SettlementState settlement)
        {
            if (settlement == null) return;
            int foodCost = Math.Max(0, settlement.residents);
            int available = settlement.ResourceAmount("meal");
            settlement.AddResource("meal", -Math.Min(foodCost, available));
            settlement.morale = Math.Max(0, Math.Min(100, settlement.morale + (available >= foodCost ? 2 : -10)));
        }

        private static bool Consume(SettlementState settlement, string id, int amount)
        {
            if (settlement.ResourceAmount(id) < amount) return false;
            settlement.AddResource(id, -amount); return true;
        }

        private static ActivityResult Success(string id, int amount, string message) { return new ActivityResult { success = true, outputId = id, outputAmount = amount, message = message }; }
        private static ActivityResult Fail(string message) { return new ActivityResult { success = false, message = message }; }
    }
}
