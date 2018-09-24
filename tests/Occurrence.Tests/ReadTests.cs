using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Occurrence.Tests.Events;
using Xunit;

namespace Occurrence.Tests
{
    public abstract class ReadTests : EventStoreTestBase
    {
        protected override async Task OnInitializedAsync()
        {
            await Subject.Append("1", Enumerable.Range(1, 70).Select(s => new EventData(new TestEvent(), DateTime.UtcNow)).ToArray(), 0);
            await Subject.Append("2", Enumerable.Range(1, 30).Select(s => new EventData(new TestEvent(), DateTime.UtcNow)).ToArray(), 0);
        }

        [Theory]
        [InlineData("1", 70)]
        [InlineData("2", 30)]
        public async Task Should_Read_Entire_Stream(string stream, int expectedCount)
        {
            var events = await Subject.Read(stream);

            events.Length.Should().Be(expectedCount);
        }

        [Fact]
        public async Task Should_Order_Events_Ascending()
        {
            var events = await Subject.Read("1");

            for (int i = 0; i < 70; i++)
            {
                events[i].EventNumber.Should().Be(i + 1);
            }
        }

        [Theory]
        [InlineData(1, 30)]
        [InlineData(10, 40)]
        [InlineData(70, 70)]
        [InlineData(1, 1)]
        public async Task Should_Read_Partial_Stream(int firstEventNumber, int lastEventNumber)
        {
            var events = await Subject.Read("1", firstEventNumber, lastEventNumber);

            events[0].EventNumber.Should().Be(firstEventNumber);
            events[events.Length - 1].EventNumber.Should().Be(lastEventNumber);
            events.Length.Should().Be(lastEventNumber - firstEventNumber + 1);
        }
    }
}