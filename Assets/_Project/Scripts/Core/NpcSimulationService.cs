using System;
using System.Collections.Generic;

namespace Veyrath.Core
{
    public sealed class NpcSimulationService
    {
        public bool SimulateOffscreen { get; set; } = true;
        public List<NpcProfile> Profiles { get; } = new List<NpcProfile>();
        public List<NpcState> States { get; } = new List<NpcState>();

        public void Register(NpcProfile profile)
        {
            if (profile == null || string.IsNullOrWhiteSpace(profile.id)) return;
            if (GetProfile(profile.id) != null) return;
            Profiles.Add(profile);
            States.Add(new NpcState { id = profile.id, regionId = profile.homeRegionId, activity = "idle" });
            SimulateTo(0);
        }

        public NpcState GetState(string id) { return States.Find(item => item.id == id); }
        public NpcProfile GetProfile(string id) { return Profiles.Find(item => item.id == id); }

        public void AdjustRelationship(string id, int amount)
        {
            NpcState state = GetState(id);
            if (state != null) state.relationship = Math.Max(-100, Math.Min(100, state.relationship + amount));
        }

        public void SimulateTo(int minute)
        {
            int boundedMinute = ((minute % (24 * 60)) + (24 * 60)) % (24 * 60);
            foreach (NpcProfile profile in Profiles)
            {
                NpcState state = GetState(profile.id);
                NpcRoutineSegment selected = null;
                foreach (NpcRoutineSegment segment in profile.routine)
                    if (boundedMinute >= segment.startMinute && boundedMinute < segment.endMinute) { selected = segment; break; }
                state.activity = selected == null ? "idle" : selected.activity;
                state.regionId = selected == null ? profile.homeRegionId : selected.regionId;
                state.lastSimulatedMinute = boundedMinute;
            }
        }

        public string SerializeStates()
        {
            List<string> values = new List<string>();
            foreach (NpcState state in States) values.Add(string.Join("|", state.id, state.regionId, state.activity, state.relationship, state.lastSimulatedMinute));
            return string.Join(";", values);
        }

        public void DeserializeStates(string serialized)
        {
            if (string.IsNullOrWhiteSpace(serialized)) return;
            foreach (string value in serialized.Split(';'))
            {
                string[] parts = value.Split('|');
                if (parts.Length != 5) continue;
                NpcState state = GetState(parts[0]);
                if (state == null) continue;
                state.regionId = parts[1];
                state.activity = parts[2];
                if (int.TryParse(parts[3], out int relationship)) state.relationship = Math.Max(-100, Math.Min(100, relationship));
                if (int.TryParse(parts[4], out int minute)) state.lastSimulatedMinute = ((minute % (24 * 60)) + (24 * 60)) % (24 * 60);
            }
        }
    }
}
