using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Occurrence.Tests.Events;
using Xunit;

namespace Occurrence.Tests.Providers
{
    public abstract class ReadTests : EventStoreTestBase
    {
        protected override async Task OnInitializedAsync()
        {
            Stream = Guid.NewGuid().ToString();

            await Subject.Append(Stream, 0, Enumerable.Range(1, 70).Select(s => new EventData(new TestEvent(), DateTime.UtcNow, new Dictionary<string, string> { ["Test"] = "Test " + s })).ToArray());
        }

        protected string Stream { get; set; }

        [Fact]
        public async Task Should_Read_Entire_Stream()
        {
            var events = await Subject.Read(Stream);

            events.Length.Should().Be(70);
        }

        [Fact]
        public async Task Should_Order_Events_Ascending()
        {
            var events = await Subject.Read(Stream);

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
            var events = await Subject.Read(Stream, firstEventNumber, lastEventNumber);

            events[0].EventNumber.Should().Be(firstEventNumber);
            events[events.Length - 1].EventNumber.Should().Be(lastEventNumber);
            events.Length.Should().Be(lastEventNumber - firstEventNumber + 1);
        }

        [Fact]
        public async Task Should_Read_Metadata()
        {
            var persistedEventDatas = await Subject.Read(Stream, 11, 11);

            persistedEventDatas.First().Metadata.Should().Contain("Test", "Test 11");
        }

        [Fact]
        public async Task Should_GetStreamVersion()
        {
            var version = await Subject.GetStreamVersion(Stream);

            version.Should().Be(70);
        }

    }
}