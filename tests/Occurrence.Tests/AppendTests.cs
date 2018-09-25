using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Occurrence.Tests.Events;
using Xunit;

namespace Occurrence.Tests
{
    public abstract class AppendTests : EventStoreTestBase
    {
        protected string Stream { get; private set; }

        protected AppendTests()
        {
            Stream = Guid.NewGuid().ToString();
        }

        [Theory]
        [InlineData(4)]
        [InlineData(7)]
        [InlineData(11)]
        [InlineData(46)]
        public async Task Should_Return_New_Current_Stream_Version(int countAndExpectedVersion)
        {
            var eventDatas = Enumerable.Range(1, countAndExpectedVersion).Select(s => new EventData(new TestEvent(), DateTime.UtcNow)).ToArray();

            var appendResult = await Subject.Append(Stream, eventDatas, 0);

            appendResult.Version.Should().Be(countAndExpectedVersion);
        }

        [Fact]
        public async Task Should_Append_Events()
        {
            var e1 = new TestEvent { Test = Guid.NewGuid().ToString() };
            var ed1 = new EventData(e1, DateTime.UtcNow);
            var e2 = new TestEvent { Test = Guid.NewGuid().ToString() };
            var ed2 = new EventData(e2, DateTime.UtcNow);

            await Subject.Append(Stream, new[] { ed1, ed2 }, 0);

            var events = await Subject.Read(Stream);

            var es1 = events[0].Event.Should().BeOfType<TestEvent>();
            es1.Subject.Test.Should().BeEquivalentTo(e1.Test);
            var es2 = events[1].Event.Should().BeOfType<TestEvent>();
            es2.Subject.Test.Should().BeEquivalentTo(e2.Test);
        }

        [Fact]
        public async Task Should_Throw_OptimisticConcurrencyException_If_ExpectedVersion_Is_Wrong()
        {
            var exception = await Assert.ThrowsAsync<OptimisticConcurrencyException>(() => Subject.Append(Stream, new[] { new EventData(new TestEvent(), DateTime.UtcNow) }, 1));

            exception.CurrentVersion.Should().Be(0);
            exception.ExpectedVersion.Should().Be(1);
        }
    }
}