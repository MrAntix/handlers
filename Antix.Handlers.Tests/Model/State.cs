using Antix.Handlers.Tests.Model.Events;
using System;

namespace Antix.Handlers.Tests.Model
{

    public sealed class State
    {
        public State(
            int total = 0,
            uint version = 0)
        {
            Total = total;
            Version = version;
        }

        public int Total { get; }
        public uint Version { get; }

        public State Apply(params IEvent[] events)
        {
            var state = this;
            var expectedSequenceNumber = Version;
            foreach (var e in events)
            {
                if (++expectedSequenceNumber != e.SequenceNumber)
                    throw new Exception("unexpected sequence number");

                state = e.Data switch
                {
                    TotalSet d => new State(d.Value, e.SequenceNumber),
                    _ => throw new ArgumentOutOfRangeException(e.GetType().Name),
                };
            }

            return state;
        }

    }
}
