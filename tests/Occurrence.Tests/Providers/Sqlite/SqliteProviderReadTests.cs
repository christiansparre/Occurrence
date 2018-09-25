using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Occurrence.Tests.Providers.Sqlite
{
    public class SqliteProviderReadTests : ReadTests
    {
        public override void Configure(EventStoreBuilder builder)
        {
            var sqliteConnection = new SqliteConnection("Data Source=:memory:");
            sqliteConnection.Open();
            builder.ConfigureDbContext(c => c.UseSqlite(sqliteConnection));
        }
    }
}