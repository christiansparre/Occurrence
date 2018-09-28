using System;

namespace Occurrence
{
    public class EventTypeMapping
    {
        public EventTypeMapping(Type type, string name)
        {
            Type = type;
            Name = name;
        }

        public Type Type { get; }
        public string Name { get; }
    }
}