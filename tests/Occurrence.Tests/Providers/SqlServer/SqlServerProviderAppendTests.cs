using System.Threading.Tasks;
using Xunit;

namespace Occurrence.Tests.Providers.SqlServer
{
    [Collection(nameof(SqlServerTestsCollection))]
    public class SqlServerProviderAppendTests : AppendTests
    {
        private readonly SqlServerTestsFixture _fixture;

        public SqlServerProviderAppendTests(SqlServerTestsFixture fixture)
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