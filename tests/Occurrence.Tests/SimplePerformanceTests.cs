﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Occurrence.Tests.Events;
using Xunit;
using Xunit.Abstractions;

namespace Occurrence.Tests
{
    public abstract class SimplePerformanceTests : EventStoreTestBase
    {
        private readonly ITestOutputHelper _out;
        protected string WarmupStream { get; } = Guid.NewGuid().ToString();
        protected string Stream { get; } = Guid.NewGuid().ToString();

        protected SimplePerformanceTests(ITestOutputHelper @out)
        {
            _out = @out;
        }

        [Fact]
        public async Task SingleEventAppends()
        {
            // Warmup
            await Subject.Append(WarmupStream, new[] { new EventData(new TestEvent(), DateTime.UtcNow) }, 0);

            var timer = Stopwatch.StartNew();
            var runfor = TimeSpan.FromSeconds(3);

            int count = 0;
            while (timer.Elapsed < runfor)
            {
                await Subject.Append(Stream, new[] { new EventData(new TestEvent(), DateTime.UtcNow) }, count);
                count++;
            }
            timer.Stop();

            _out.WriteLine($"Appended {count} events in {timer.Elapsed:g}");
            _out.WriteLine($"Throughput was {count / timer.Elapsed.TotalSeconds:N0} events/s");
        }

        [Theory]
        [InlineData(3)]
        [InlineData(8)]
        [InlineData(15)]
        public async Task EventBatchesAppends(int batchCount)
        {
            // Warmup
            await Subject.Append(WarmupStream, new[] { new EventData(new TestEvent(), DateTime.UtcNow) }, 0);

            var timer = Stopwatch.StartNew();
            var runfor = TimeSpan.FromSeconds(3);

            int count = 0;
            while (timer.Elapsed < runfor)
            {
                var eventDatas = Enumerable.Range(1, batchCount).Select(s => new EventData(new TestEvent(), DateTime.UtcNow)).ToArray();

                await Subject.Append(Stream, eventDatas, count);
                count = count + batchCount;
            }
            timer.Stop();

            _out.WriteLine($"Appended {count} events in batches of {batchCount} in {timer.Elapsed:g}");
            _out.WriteLine($"Throughput was {count / timer.Elapsed.TotalSeconds:N0} events/s");
        }
    }
}