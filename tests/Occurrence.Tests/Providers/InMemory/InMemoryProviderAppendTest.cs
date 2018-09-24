using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Occurrence.Tests.Providers.InMemory
{
    public class InMemoryProviderAppendTest : AppendTests
    {
        public override Task<DbContextOptions<EventDbContext>> GetOptions()
        {
            var options = new DbContextOptionsBuilder<EventDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return Task.FromResult(options);
        }
    }
}