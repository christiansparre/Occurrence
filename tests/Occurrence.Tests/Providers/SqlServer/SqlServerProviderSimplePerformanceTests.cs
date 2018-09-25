using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Occurrence.Tests.Providers.SqlServer
{
    [Collection(nameof(SqlServerTestsCollection))]
    public class SqlServerProviderSimplePerformanceTests : SimplePerformanceTests
    {
        private readonly SqlServerTestsFixture _fixture;

        public SqlServerProviderSimplePerformanceTests(SqlServerTestsFixture fixture, ITestOutputHelper @out) : base(@out)
        {
            _fixture = fixture;
        }

        public override void Configure(EventStoreBuilder builder)
        {
            _fixture.Configure(builder);
        }

        public override Task OnDisposeAsync()
        {
            return _fixture.Cleanup(Stream, WarmupStream);
        }
    }
}