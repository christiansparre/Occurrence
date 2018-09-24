using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Occurrence
{
    public class EventStore
    {
        private readonly DbContextOptions _options;

        public EventStore(DbContextOptions options)
        {
            _options = options;
        }

        public async Task<AppendResult> Append(string stream, EventData[] events, int expectedVersion)
        {
            var version = expectedVersion;

            var entities = events.Select(s => new SerializedEventEntity
            {
                Stream = stream,
                EventNumber = ++version,
                Timestamp = s.Timestamp.Ticks,
                EventType = s.Event.GetType().AssemblyQualifiedName,
                Event = JsonConvert.SerializeObject(s.Event),
                Metadata = JsonConvert.SerializeObject(s.Metadata)
            }).ToArray();

            using (var db = new EventDbContext(_options))
            {
                var currentVersion = await db.Events
                    .Where(s => s.Stream == stream)
                    .OrderByDescending(o => o.EventNumber)
                    .Select(s => s.EventNumber)
                    .FirstOrDefaultAsync();

                if (currentVersion != expectedVersion)
                {
                    throw new OptimisticConcurrencyException(expectedVersion, currentVersion);
                }

                await db.Events.AddRangeAsync(entities);
                await db.SaveChangesAsync();
            }

            return new AppendResult(version);
        }

        public async Task<PersistedEventData[]> Read(string stream, int firstEventNumber = 1, int lastEventNumber = int.MaxValue)
        {
            using (var db = new EventDbContext(_options))
            {
                var entities = await db.Events
                    .Where(e => e.Stream == stream && e.EventNumber >= firstEventNumber && e.EventNumber <= lastEventNumber)
                    .OrderBy(o => o.EventNumber)
                    .ToListAsync();

                return entities.Select(s => new PersistedEventData(
                    s.EventNumber,
                    s.EventType,
                    new DateTime(s.Timestamp, DateTimeKind.Utc),
                    JsonConvert.DeserializeObject(s.Event, Type.GetType(s.EventType)),
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(s.Metadata)
                )).ToArray();
            }
        }
    }
}