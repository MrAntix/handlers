namespace Antix.Handlers.Tests.Model
{
    public interface IEvent
    {
        object Data { get; }
        uint SequenceNumber { get; }
    }
}
