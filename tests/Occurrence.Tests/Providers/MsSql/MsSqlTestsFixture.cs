using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Occurrence.Tests.Providers.MsSql
{
    public class MsSqlTestsFixture
    {
        private IConfiguration _config;

        public MsSqlTestsFixture()
        {
            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("MsSqlTestOptions:ConnectionString","Server=.;Database=Occurrence_Tests;Trusted_Connection=True;"),
                    new KeyValuePair<string, string>("MsSqlTestOptions:DropDatabase","False")
                })
                .AddEnvironmentVariables()
                .Build();

            Options = new DbContextOptionsBuilder<EventDbContext>()
                .UseSqlServer(_config.GetValue<string>("MsSqlTestOptions:ConnectionString"))
                .Options;
        }

        public DbContextOptions<EventDbContext> Options { get; set; }

        public async Task Cleanup(params string[] streams)
        {
            using (var db = new EventDbContext(Options))
            {
                db.Events.RemoveRange(db.Events.Where(s => streams.Contains(s.Stream)));
                await db.SaveChangesAsync();
            }
        }
    }
}
