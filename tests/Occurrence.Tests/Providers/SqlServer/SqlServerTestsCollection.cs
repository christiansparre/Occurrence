using Xunit;

namespace Occurrence.Tests.Providers.SqlServer
{
    [CollectionDefinition(nameof(SqlServerTestsCollection))]
    public class SqlServerTestsCollection : ICollectionFixture<SqlServerTestsFixture> { }
}