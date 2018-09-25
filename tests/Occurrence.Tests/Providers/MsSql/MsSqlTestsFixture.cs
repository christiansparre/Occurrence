﻿using System;
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
        private readonly IConfigurationRoot _config;
        private readonly DbContextOptions<EventDbContext> _options;

        public MsSqlTestsFixture()
        {
            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("MsSqlTestOptions:ConnectionString","Server=.;Database=Occurrence_Tests;Trusted_Connection=True;")
                })
                .AddEnvironmentVariables()
                .Build();

            _options = new DbContextOptionsBuilder<EventDbContext>()
                .UseSqlServer(_config.GetValue<string>("MsSqlTestOptions:ConnectionString"))
                .Options;

            using (var db = new EventDbContext(_options))
            {
                db.Database.EnsureCreated();
            }
        }

        public async Task Cleanup(params string[] streams)
        {
            using (var db = new EventDbContext(_options))
            {
                db.Events.RemoveRange(db.Events.Where(s => streams.Contains(s.Stream)));
                await db.SaveChangesAsync();
            }
        }

        public void Configure(EventStoreBuilder builder)
        {
            builder.ConfigureDbContext(o => o.UseSqlServer(_config.GetValue<string>("MsSqlTestOptions:ConnectionString")));
        }
    }
}
