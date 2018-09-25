using System;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Occurrence.Tests.Providers.InMemory
{
    public class InMemorySimplePerformanceTests : SimplePerformanceTests
    {
        public override void Configure(EventStoreBuilder builder)
        {
            builder
                .ConfigureDbContext(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()));
        }

        public InMemorySimplePerformanceTests(ITestOutputHelper @out) : base(@out)
        {
        }
    }
}