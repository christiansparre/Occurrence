using System;

namespace Occurrence
{
    public class EventTypMapping
    {
        public EventTypMapping(Type type, string name)
        {
            Type = type;
            Name = name;
        }

        public Type Type { get; }
        public string Name { get; }
    }
}