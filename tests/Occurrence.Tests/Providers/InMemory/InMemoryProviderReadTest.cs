using System;
using Microsoft.EntityFrameworkCore;

namespace Occurrence.Tests.Providers.InMemory
{
    public class InMemoryProviderReadTest : ReadTests
    {
        public override void Configure(EventStoreBuilder builder)
        {
            builder
                .ConfigureDbContext(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()));
        }
    }
}