using System;

namespace Veyrath.Core
{
    [Serializable]
    public sealed class FactionReputation
    {
        public int eddaFaction;

        public void Adjust(int amount)
        {
            eddaFaction = Math.Max(-100, Math.Min(100, eddaFaction + amount));
        }
    }
}
