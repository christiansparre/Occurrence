using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Occurrence.Tests.Providers.MsSql
{
    public class MsSqlProviderSimplePerformanceTests : SimplePerformanceTests, IClassFixture<MsSqlTestsFixture>
    {
        private readonly MsSqlTestsFixture _fixture;

        public MsSqlProviderSimplePerformanceTests(MsSqlTestsFixture fixture, ITestOutputHelper @out) : base(@out)
        {
            _fixture = fixture;
        }

        public override Task<DbContextOptions<EventDbContext>> GetOptions()
        {
            return Task.FromResult(_fixture.Options);
        }

        public override Task OnDisposeAsync()
        {
            return _fixture.Cleanup(Stream, WarmupStream);
        }
    }
}