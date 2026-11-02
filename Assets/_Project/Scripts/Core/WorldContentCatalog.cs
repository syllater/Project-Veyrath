using System;

namespace Veyrath.Core
{
    public static class WorldContentCatalog
    {
        private static RegionContentDefinition Content(string settlement, string ruin, string dungeon, string quest, string npc, string faction, string boss, string item, string resource, string secret, string worldEvent)
        {
            return new RegionContentDefinition
            {
                settlements = new[] { settlement }, ruins = new[] { ruin }, dungeons = new[] { dungeon }, quests = new[] { quest }, npcs = new[] { npc },
                factions = new[] { faction }, bosses = new[] { boss }, items = new[] { item }, resources = new[] { resource }, secrets = new[] { secret }, worldEvents = new[] { worldEvent }
            };
        }

        public static RegionContentDefinition FirstLight { get { return Content("First Light Camp", "Ruined Arch", "Wraith Hollow", "first-light-embers", "Edda", "first-light-wardens", "Ashen Wraith", "Warding Charm", "Heartstone", "Lantern Cache", "First Light Muster"); } }
        public static RegionContentDefinition AshenReach { get { return Content("Cinderwatch", "Ashfall Chapel", "The Soot Tunnels", "ashen-road", "Mara Coalhand", "Cinder Guild", "The Cinder Maw", "Coalbrand Axe", "Cinder Ore", "Blackglass Vault", "Ash Storm"); } }
        public static RegionContentDefinition VerdantReach { get { return Content("Mossgrove", "Rootbound Shrine", "Thornroot Grotto", "verdant-oath", "Sella Greenbough", "Verdant Circle", "Elder Briar", "Living Bow", "Moonleaf", "Whispering Stump", "Bloomtide"); } }
        public static RegionContentDefinition Frostmere { get { return Content("Frostmere Hold", "Frozen Observatory", "Glacier Vein", "frostmere-signal", "Orin Vaal", "Merebound", "The White Stag", "Frostglass Spear", "Frostsalt", "Under-Ice Bell", "Long Night"); } }
        public static RegionContentDefinition Ironroot { get { return Content("Ironroot Foundry", "Collapsed Lift", "Deep Engine", "ironroot-debt", "Hest Brassveil", "Ironroot Compact", "The Buried Colossus", "Gravhammer", "Star Iron", "Clockwork Seed", "Foundry Revolt"); } }
        public static RegionContentDefinition StarfallDesert { get { return Content("Astral Caravan", "Fallen Skybridge", "Sunken Observatory", "starfall-map", "Nima Dustwalker", "Dune Compact", "The Glass Serpent", "Meteor Sickle", "Sunstone", "Singing Dune", "Meteor Shower"); } }
        public static RegionContentDefinition SunkenCoast { get { return Content("Tidebreak", "Salt Cathedral", "Drowned Vault", "sunken-tide", "Corin Tidebound", "Tidebound League", "The Drowned Crown", "Tideglass Trident", "Pearl Kelp", "Blue Door", "King Tide"); } }
        public static RegionContentDefinition Veilwood { get { return Content("Veilwood Hamlet", "Moonlit Menhir", "The Veil Below", "veilwood-accord", "Ilyra Veilborn", "Veil Kin", "The Hollow Queen", "Veilthread Cloak", "Gloam Resin", "Nameless Grove", "Veil Eclipse"); } }
    }
}
