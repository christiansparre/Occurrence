namespace Occurrence.Tests.Events
{
    [Event("TestEvent")]
    public class TestEvent
    {
        public string Test { get; set; }
    }
}