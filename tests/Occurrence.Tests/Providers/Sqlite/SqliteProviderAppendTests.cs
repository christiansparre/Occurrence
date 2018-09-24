using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Occurrence.Tests.Providers.Sqlite
{
    public class SqliteProviderAppendTests : AppendTests
    {
        public override async Task<DbContextOptions<EventDbContext>> GetOptions()
        {
            var sqliteConnection = new SqliteConnection("Data Source=:memory:");
            await sqliteConnection.OpenAsync();
            var options = new DbContextOptionsBuilder<EventDbContext>()
                .UseSqlite(sqliteConnection)
                .Options;

            return options;
        }
    }
}