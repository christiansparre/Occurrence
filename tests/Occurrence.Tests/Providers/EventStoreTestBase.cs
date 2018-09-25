using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Occurrence.Tests.Events;
using Xunit;

namespace Occurrence.Tests
{
    public abstract class EventStoreTestBase : IAsyncLifetime
    {
        public abstract void Configure(EventStoreBuilder builder);

        public virtual Task OnDisposeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            var eventStoreBuilder = new EventStoreBuilder();

            Configure(eventStoreBuilder);
            eventStoreBuilder.MapEventsFromAssemblyOf<TestEvent>();

            var eventStore = eventStoreBuilder.Build(out var options);
            Options = options;

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

            Subject = eventStore;
            await OnInitializedAsync();
        }

        public IEventStore Subject { get; private set; }

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