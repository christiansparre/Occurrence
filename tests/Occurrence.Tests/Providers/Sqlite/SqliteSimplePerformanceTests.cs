using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Occurrence.Tests.Providers.Sqlite
{
    public class SqliteSimplePerformanceTests : SimplePerformanceTests
    {
        public override void Configure(EventStoreBuilder builder)
        {
            var sqliteConnection = new SqliteConnection("Data Source=:memory:");
            sqliteConnection.Open();
            builder.ConfigureDbContext(c => c.UseSqlite(sqliteConnection));
        }

        public SqliteSimplePerformanceTests(ITestOutputHelper @out) : base(@out)
        {
        }
    }
}