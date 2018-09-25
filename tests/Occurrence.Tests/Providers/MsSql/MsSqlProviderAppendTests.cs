﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Occurrence.Tests.Providers.MsSql
{
    public class MsSqlProviderAppendTests : AppendTests, IClassFixture<MsSqlTestsFixture>
    {
        private readonly MsSqlTestsFixture _fixture;

        public MsSqlProviderAppendTests(MsSqlTestsFixture fixture)
        {
            _fixture = fixture;
        }

        public override Task<DbContextOptions<EventDbContext>> GetOptions()
        {
            return Task.FromResult(_fixture.Options);
        }

        public override Task OnDisposeAsync()
        {
            return _fixture.Cleanup(Stream);
        }
    }
}