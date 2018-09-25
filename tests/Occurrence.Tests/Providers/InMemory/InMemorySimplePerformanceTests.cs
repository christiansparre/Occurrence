using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Occurrence.Tests.Providers.InMemory
{
    public class InMemorySimplePerformanceTests : SimplePerformanceTests
    {
        public override Task<DbContextOptions<EventDbContext>> GetOptions()
        {
            var options = new DbContextOptionsBuilder<EventDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return Task.FromResult(options);
        }

        public InMemorySimplePerformanceTests(ITestOutputHelper @out) : base(@out)
        {
        }
    }
}