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
        
        public override void Configure(EventStoreBuilder builder)
        {
            _fixture.Configure(builder);
        }

        public override Task OnDisposeAsync()
        {
            return _fixture.Cleanup(Stream);
        }
    }
}