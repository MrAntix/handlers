using System;

namespace Antix.Handlers.Tests.Model.Commands
{
    public interface ICommandWrapper
    {
        string UserId { get; }
        uint SequenceNumber { get; }
        DateTimeOffset On { get; }
    }
}