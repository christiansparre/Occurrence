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
                await db.Database.EnsureCreatedAsync();
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