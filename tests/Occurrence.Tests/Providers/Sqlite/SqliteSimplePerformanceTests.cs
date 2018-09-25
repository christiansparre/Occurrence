﻿using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Occurrence.Tests.Providers.Sqlite
{
    public class SqliteSimplePerformanceTests : SimplePerformanceTests
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

        public SqliteSimplePerformanceTests(ITestOutputHelper @out) : base(@out)
        {
        }
    }
}