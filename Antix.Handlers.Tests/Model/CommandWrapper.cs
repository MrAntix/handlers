using System;

namespace Antix.Handlers.Tests.Model
{
    public sealed class CommandWrapper<TCommand> : ICommandWrapper
    {
        public CommandWrapper(
            TCommand command,
            string userId,
            uint sequenceNumber,
            DateTimeOffset? on = null)
        {
            Command = command;
            UserId = userId;
            SequenceNumber = sequenceNumber;
            On = on ?? DateTimeOffset.UtcNow;
        }

        public TCommand Command { get; }
        public string UserId { get; }
        public uint SequenceNumber { get; }
        public DateTimeOffset On { get; }
    }
}
