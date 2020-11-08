namespace Antix.Handlers.Tests.Model.Events
{
    public interface IEvent
    {
        object Data { get; }
        uint SequenceNumber { get; }
    }
}
