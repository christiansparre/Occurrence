using System.Threading.Tasks;
using Xunit;

namespace Occurrence.Tests.Providers.SqlServer
{
    [Collection(nameof(SqlServerTestsCollection))]
    public class SqlServerProviderReadTests : ReadTests
    {
        private readonly SqlServerTestsFixture _fixture;

        public SqlServerProviderReadTests(SqlServerTestsFixture fixture)
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