using System;

namespace Antix.Handlers.Tests.Model
{
    public static class CommandWrapper
    {
        public static CommandWrapper<TCommand> From<TCommand>(
            TCommand command,
            string userId = null, uint sequenceNumber = 0,
            DateTimeOffset? on = null)
        {
            return new CommandWrapper<TCommand>(
                command,
                userId, sequenceNumber,
                on);
        }
    }

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

        object ICommandWrapper.Command => Command;
    }
}
