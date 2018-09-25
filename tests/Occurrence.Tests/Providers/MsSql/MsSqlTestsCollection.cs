using Xunit;

namespace Occurrence.Tests.Providers.MsSql
{
    [CollectionDefinition(nameof(MsSqlTestsCollection))]
    public class MsSqlTestsCollection : ICollectionFixture<MsSqlTestsFixture> { }
}