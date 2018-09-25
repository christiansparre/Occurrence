using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Occurrence
{
    public class EventStore : IEventStore
    {
        private readonly DbContextOptions _options;
        private readonly Dictionary<string, Type> _eventNameToEventTypeMap;
        private readonly Dictionary<Type, string> _eventTypeToEventNameMap;

        public EventStore(DbContextOptions options, EventTypeMapping[] eventTypeMappings)
        {
            _options = options;
            _eventTypeToEventNameMap = eventTypeMappings.ToDictionary(d => d.Type, d => d.Name);
            _eventNameToEventTypeMap = eventTypeMappings.ToDictionary(d => d.Name, d => d.Type);
        }

        public async Task<AppendResult> Append(string stream, int expectedVersion, params EventData[] events)
        {
            var version = expectedVersion;

            var entities = events.Select(s => new SerializedEventEntity
            {
                Stream = stream,
                EventNumber = ++version,
                Timestamp = s.Timestamp.Ticks,
                EventType = GetEventTypeName(s.Event),
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

        private string GetEventTypeName(object @event)
        {
            if (!_eventTypeToEventNameMap.TryGetValue(@event.GetType(), out var name))
            {
                throw new NotSupportedException($"Event type {@event.GetType()} is not mapped");
            }
            return name;
        }

        private Type GetEventType(string name)
        {
            if (!_eventNameToEventTypeMap.TryGetValue(name, out var type))
            {
                throw new NotSupportedException($"Event name {name} is not mapped");
            }
            return type;
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
                    JsonConvert.DeserializeObject(s.Event, GetEventType(s.EventType)),
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(s.Metadata)
                )).ToArray();
            }
        }
    }
}