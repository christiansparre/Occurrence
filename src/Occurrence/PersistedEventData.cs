using System;
using System.Collections.Generic;

namespace Occurrence
{
    public class PersistedEventData
    {
        public PersistedEventData(int eventNumber, string eventType, DateTime timestamp, object @event, Dictionary<string, string> metadata)
        {
            EventNumber = eventNumber;
            EventType = eventType;
            Timestamp = timestamp;
            Event = @event;
            Metadata = metadata;
        }

        public int EventNumber { get; }
        public string EventType { get; }
        public DateTime Timestamp { get; }
        public object Event { get; }
        public Dictionary<string, string> Metadata { get; }
    }
}