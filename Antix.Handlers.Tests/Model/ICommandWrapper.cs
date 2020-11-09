using System;

namespace Antix.Handlers.Tests.Model
{
    public interface ICommandWrapper
    {
        object Command { get; }
        string UserId { get; }
        uint SequenceNumber { get; }
        DateTimeOffset On { get; }
    }
}