using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Occurrence.Tests.Providers.MsSql
{
    [CollectionDefinition(nameof(MsSqlTestsCollection))]
    public class MsSqlTestsCollection : ICollectionFixture<MsSqlTestsFixture> { }


    public class MsSqlTestsFixture
    {
        public MsSqlTestsFixture()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("MsSqlTestOptions:ConnectionString","Server=.;Database=Occurrence_Tests;Trusted_Connection=True;")
                })
                .AddEnvironmentVariables()
                .Build();

            Options = new DbContextOptionsBuilder<EventDbContext>()
                .UseSqlServer(config.GetValue<string>("MsSqlTestOptions:ConnectionString"))
                .Options;

            using (var db = new EventDbContext(Options))
            {
                db.Database.EnsureCreated();
            }
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
