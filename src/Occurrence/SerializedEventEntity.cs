namespace Occurrence
{
    public class SerializedEventEntity
    {
        public long Id { get; set; }
        public string Stream { get; set; }
        public int EventNumber { get; set; }
        public string EventType { get; set; }
        public long Timestamp { get; set; }
        public string Event { get; set; }
        public string Metadata { get; set; }
    }
}