using System;
using System.Collections.Generic;

namespace Occurrence
{
    public class EventData
    {
        public EventData(object @event, DateTime timestamp, Dictionary<string, string> metadata = null)
        {
            Event = @event;
            Timestamp = timestamp;
            Metadata = metadata ?? new Dictionary<string, string>();

        }

        public object Event { get; }
        public DateTime Timestamp { get; }
        public Dictionary<string, string> Metadata { get; }
    }
}