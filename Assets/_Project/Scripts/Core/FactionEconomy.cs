using System;
using System.Collections.Generic;

namespace Veyrath.Core
{
    public enum TreatyType { TradePact, NonAggression, Alliance }

    [Serializable]
    public sealed class FactionDefinition
    {
        public string id;
        public string displayName;
        public string[] homeRegions = Array.Empty<string>();
    }

    [Serializable]
    public sealed class FactionState
    {
        public string factionId;
        public int reputation;
        public int influence;
        public int treasury;
        public bool atWar;
    }

    [Serializable]
    public sealed class RegionalStockpile
    {
        public string regionId;
        public string resourceId;
        public int amount;
    }

    [Serializable]
    public sealed class TradeRouteState
    {
        public string id;
        public string ownerFactionId;
        public string originRegionId;
        public string destinationRegionId;
        public string resourceId;
        public int capacityPerDay;
        public int riskPercent;
        public bool active = true;
    }

    [Serializable]
    public sealed class ConflictState
    {
        public string id;
        public string attackerFactionId;
        public string defenderFactionId;
        public string regionId;
        public int intensity;
        public bool active = true;
    }

    [Serializable]
    public sealed class TreatyState
    {
        public string id;
        public string factionA;
        public string factionB;
        public TreatyType type;
        public int expiresDay;
        public bool active = true;
    }

    [Serializable]
    public sealed class WorldReactionState
    {
        public string id;
        public string regionId;
        public string message;
        public int severity;
        public int day;
    }

    public sealed class FactionWorldSimulation
    {
        public readonly List<FactionDefinition> Definitions = new List<FactionDefinition>();
        public readonly List<FactionState> Factions = new List<FactionState>();
        public readonly List<RegionalStockpile> Stockpiles = new List<RegionalStockpile>();
        public readonly List<TradeRouteState> TradeRoutes = new List<TradeRouteState>();
        public readonly List<ConflictState> Conflicts = new List<ConflictState>();
        public readonly List<TreatyState> Treaties = new List<TreatyState>();
        public readonly List<WorldReactionState> Reactions = new List<WorldReactionState>();
        public readonly List<string> Territories = new List<string>();
        public int CurrentDay { get; private set; }

        public void RegisterFaction(FactionDefinition definition)
        {
            if (definition == null || string.IsNullOrWhiteSpace(definition.id) || GetFaction(definition.id) != null) return;
            Definitions.Add(definition);
            Factions.Add(new FactionState { factionId = definition.id });
        }

        public FactionState GetFaction(string id) { return Factions.Find(item => item.factionId == id); }
        public bool SetTerritory(string regionId, string factionId)
        {
            if (string.IsNullOrWhiteSpace(regionId) || GetFaction(factionId) == null) return false;
            string entry = regionId + "=" + factionId;
            for (int i = Territories.Count - 1; i >= 0; i--) if (Territories[i].StartsWith(regionId + "=")) Territories.RemoveAt(i);
            Territories.Add(entry);
            return true;
        }

        public string TerritoryOwner(string regionId)
        {
            string entry = Territories.Find(item => item.StartsWith(regionId + "="));
            return entry == null ? string.Empty : entry.Substring(regionId.Length + 1);
        }

        public void AddStock(string regionId, string resourceId, int amount)
        {
            if (amount == 0) return;
            RegionalStockpile stock = Stockpiles.Find(item => item.regionId == regionId && item.resourceId == resourceId);
            if (stock == null) { stock = new RegionalStockpile { regionId = regionId, resourceId = resourceId }; Stockpiles.Add(stock); }
            stock.amount = Math.Max(0, stock.amount + amount);
        }

        public int Stock(string regionId, string resourceId)
        {
            RegionalStockpile stock = Stockpiles.Find(item => item.regionId == regionId && item.resourceId == resourceId);
            return stock == null ? 0 : stock.amount;
        }

        public bool AddTradeRoute(TradeRouteState route)
        {
            if (route == null || string.IsNullOrWhiteSpace(route.id) || GetFaction(route.ownerFactionId) == null || TradeRoutes.Exists(item => item.id == route.id)) return false;
            route.capacityPerDay = Math.Max(1, route.capacityPerDay); route.riskPercent = Math.Max(0, Math.Min(100, route.riskPercent));
            TradeRoutes.Add(route); return true;
        }

        public bool SignTreaty(TreatyState treaty)
        {
            if (treaty == null || treaty.factionA == treaty.factionB || GetFaction(treaty.factionA) == null || GetFaction(treaty.factionB) == null || HasTreaty(treaty.factionA, treaty.factionB)) return false;
            treaty.active = true; treaty.expiresDay = Math.Max(CurrentDay + 1, treaty.expiresDay); Treaties.Add(treaty); return true;
        }

        public bool HasTreaty(string a, string b)
        {
            return Treaties.Exists(item => item.active && ((item.factionA == a && item.factionB == b) || (item.factionA == b && item.factionB == a)) && item.expiresDay >= CurrentDay);
        }

        public bool DeclareConflict(string id, string attacker, string defender, string regionId)
        {
            if (string.IsNullOrWhiteSpace(id) || attacker == defender || GetFaction(attacker) == null || GetFaction(defender) == null || HasTreaty(attacker, defender) || Conflicts.Exists(item => item.id == id)) return false;
            Conflicts.Add(new ConflictState { id = id, attackerFactionId = attacker, defenderFactionId = defender, regionId = regionId, intensity = 1 });
            GetFaction(attacker).atWar = true; GetFaction(defender).atWar = true;
            return true;
        }

        public void AdjustReputation(string factionId, int amount)
        {
            FactionState faction = GetFaction(factionId); if (faction == null) return;
            faction.reputation = Math.Max(-100, Math.Min(100, faction.reputation + amount));
        }

        public bool ApplyPlayerAction(string actionId, string factionId, string regionId)
        {
            FactionState faction = GetFaction(factionId); if (faction == null || string.IsNullOrWhiteSpace(actionId)) return false;
            int severity = 1;
            if (actionId == "defend") { faction.influence += 5; AdjustReputation(factionId, 10); AddStock(regionId, "security", 1); }
            else if (actionId == "trade") { faction.treasury += 10; AddStock(regionId, "trade-goods", 1); }
            else if (actionId == "sabotage") { faction.influence = Math.Max(0, faction.influence - 3); AdjustReputation(factionId, -15); severity = 2; }
            else return false;
            Reactions.Add(new WorldReactionState { id = actionId + "-" + CurrentDay + "-" + Reactions.Count, regionId = regionId, message = actionId + " changed the balance in " + regionId, severity = severity, day = CurrentDay });
            return true;
        }

        public void AdvanceDay()
        {
            CurrentDay++;
            foreach (TradeRouteState route in TradeRoutes)
            {
                if (!route.active || Stock(route.originRegionId, route.resourceId) <= 0) continue;
                int delivered = Math.Min(route.capacityPerDay, Stock(route.originRegionId, route.resourceId));
                if (route.riskPercent > 0 && ((CurrentDay * 31 + route.id.Length) % 100) < route.riskPercent) delivered = 0;
                AddStock(route.originRegionId, route.resourceId, -delivered); AddStock(route.destinationRegionId, route.resourceId, delivered);
                FactionState owner = GetFaction(route.ownerFactionId); if (owner != null) owner.treasury += delivered;
            }
            foreach (TreatyState treaty in Treaties) if (treaty.expiresDay < CurrentDay) treaty.active = false;
        }
    }
}
