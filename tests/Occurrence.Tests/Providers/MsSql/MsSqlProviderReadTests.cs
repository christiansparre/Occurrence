using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Occurrence.Tests.Providers.MsSql
{
    [Collection(nameof(MsSqlTestsCollection))]
    public class MsSqlProviderReadTests : ReadTests
    {
        private readonly MsSqlTestsFixture _fixture;

        public MsSqlProviderReadTests(MsSqlTestsFixture fixture)
        {
            _fixture = fixture;
        }

        public override Task<DbContextOptions<EventDbContext>> GetOptions()
        {
            return Task.FromResult(_fixture.Options);
        }

        public override Task OnDisposeAsync()
        {
            return _fixture.Cleanup(Stream);
        }
    }
}