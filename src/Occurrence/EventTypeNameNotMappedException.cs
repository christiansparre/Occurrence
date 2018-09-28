using System;

namespace Occurrence
{
    public class EventTypeNameNotMappedException : Exception
    {
        public string Name { get; set; }

        public EventTypeNameNotMappedException(string name) : base($"Event type {name} is not mapped")
        {
            Name = name;
        }
    }
}