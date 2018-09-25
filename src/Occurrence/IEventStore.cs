using System.Threading.Tasks;

namespace Occurrence
{
    public interface IEventStore
    {
        Task<AppendResult> Append(string stream, EventData[] events, int expectedVersion);
        Task<PersistedEventData[]> Read(string stream, int firstEventNumber = 1, int lastEventNumber = int.MaxValue);
    }
}