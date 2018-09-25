using System;
using Occurrence.Tests.Events;
using Xunit;

namespace Occurrence.Tests
{
    public class EventStoreBuilderTests
    {
        private readonly EventStoreBuilder _subject;

        public EventStoreBuilderTests()
        {
            _subject = new EventStoreBuilder();
        }

        [Fact]
        public void Should_Throw_NotSupportedException_If_EventType_Is_Mapped_Multiple_Times()
        {
            // TODO this test does not cover the scenario very well, it should probably
            // TODO include events from multiple assemblies to test different events with the same name
            Assert.Throws<NotSupportedException>(() => _subject
                .MapEventsFromAssemblyOf<TestEvent>()
                .MapEventsFromAssemblyOf<TestEvent>()
                .Build());
        }
    }
}