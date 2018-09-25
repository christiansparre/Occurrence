using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Occurrence
{
    public class EventStoreBuilder
    {
        private DbContextOptionsBuilder<EventDbContext> _options;
        private readonly List<EventTypMapping> _eventTypMappings = new List<EventTypMapping>();

        public EventStoreBuilder ConfigureDbContext(Action<DbContextOptionsBuilder<EventDbContext>> configurator)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EventDbContext>();
            configurator(optionsBuilder);
            _options = optionsBuilder;
            return this;
        }

        public EventStoreBuilder MapEventsFromAssemblyOf<TEvent>()
        {
            var types = typeof(TEvent).Assembly.GetExportedTypes().Where(t => t.GetCustomAttribute<EventAttribute>() != null);

            foreach (var type in types)
            {
                _eventTypMappings.Add(new EventTypMapping(type, type.GetCustomAttribute<EventAttribute>().Name));
            }

            return this;
        }

        public IEventStore Build()
        {
            return Build(out _);
        }

        public IEventStore Build(out DbContextOptions<EventDbContext> options)
        {
            var typeGrouping = _eventTypMappings.GroupBy(g => g.Type).Where(a => a.Count() > 1).ToList();
            if (typeGrouping.Any())
            {
                throw new NotSupportedException($"Multiple instances of event type mapping found for types {string.Join(", ", typeGrouping.Select(s => s.Key.Name))}");
            }

            var nameMapping = _eventTypMappings.GroupBy(g => g.Name).Where(a => a.Count() > 1).ToList();
            if (nameMapping.Any())
            {
                throw new NotSupportedException($"Multiple instances of event type mapping found for names {string.Join(", ", nameMapping.Select(s => s.Key))}");
            }
            options = _options.Options;
            return new EventStore(_options.Options, _eventTypMappings);
        }
    }
}