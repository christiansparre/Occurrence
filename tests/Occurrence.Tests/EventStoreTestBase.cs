using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Occurrence.Tests
{
    public abstract class EventStoreTestBase : IAsyncLifetime
    {
        public abstract Task<DbContextOptions<EventDbContext>> GetOptions();

        public virtual Task OnDisposeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            Options = await GetOptions();

            using (var db = new EventDbContext(Options))
            {
                try
                {
                    await db.Database.EnsureCreatedAsync();
                }
                catch (SqlException e) when (e.Number == 1801)
                {
                    // Catch race condition when running tests and the database does not exist
                    // causes the EnsureCreatedAsync to fail
                }
            }

            Subject = new EventStore(Options);
            await OnInitializedAsync();
        }

        public EventStore Subject { get; private set; }

        protected DbContextOptions<EventDbContext> Options { get; private set; }

        public Task DisposeAsync()
        {
            return OnDisposeAsync();
        }

        protected virtual Task OnInitializedAsync()
        {
            return Task.CompletedTask;
        }
    }
}