using NUnit.Framework;
using Veyrath.Core;

namespace Veyrath.Tests
{
    public sealed class NpcSimulationTests
    {
        private static NpcSimulationService CreateService()
        {
            NpcSimulationService service = new NpcSimulationService();
            service.Register(new NpcProfile
            {
                id = "edda", displayName = "Edda", homeRegionId = "first-light",
                routine = new[]
                {
                    new NpcRoutineSegment { startMinute = 0, endMinute = 360, activity = "sleep", regionId = "first-light" },
                    new NpcRoutineSegment { startMinute = 360, endMinute = 1080, activity = "work", regionId = "first-light" }
                }
            });
            return service;
        }

        [Test]
        public void RoutineSelectionIsDeterministic()
        {
            NpcSimulationService service = CreateService();

            service.SimulateTo(120);
            Assert.That(service.GetState("edda").activity, Is.EqualTo("sleep"));
            service.SimulateTo(600);
            Assert.That(service.GetState("edda").activity, Is.EqualTo("work"));
        }

        [Test]
        public void RelationshipChangesContextLine()
        {
            NpcSimulationService service = CreateService();
            service.AdjustRelationship("edda", 20);

            Assert.That(service.GetState("edda").ContextLine(service.GetProfile("edda")), Does.Contain("count on you"));
        }

        [Test]
        public void NpcStateRoundTrips()
        {
            NpcSimulationService source = CreateService();
            source.SimulateTo(600);
            source.AdjustRelationship("edda", 15);
            string saved = source.SerializeStates();
            NpcSimulationService restored = CreateService();

            restored.DeserializeStates(saved);

            Assert.That(restored.GetState("edda").activity, Is.EqualTo("work"));
            Assert.That(restored.GetState("edda").relationship, Is.EqualTo(15));
        }

        [Test]
        public void DuplicateRegistrationDoesNotDuplicateState()
        {
            NpcSimulationService service = CreateService();
            service.Register(service.GetProfile("edda"));
            Assert.That(service.Profiles.Count, Is.EqualTo(1));
            Assert.That(service.States.Count, Is.EqualTo(1));
        }
    }
}
