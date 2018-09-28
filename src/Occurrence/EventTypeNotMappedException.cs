using System;

namespace Occurrence
{
    public class EventTypeNotMappedException : Exception
    {
        public Type Type { get; set; }

        public EventTypeNotMappedException(Type type) : base($"Event type {type} is not mapped")
        {
            Type = type;
        }

    }
}