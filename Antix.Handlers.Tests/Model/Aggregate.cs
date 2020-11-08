using Antix.Handlers.Tests.Model.Events;
using System;
using System.Collections.Generic;

namespace Antix.Handlers.Tests.Model
{
    public sealed class Aggregate 
    {
        public Aggregate(
            State state)
        {
            State = state;
        }

        public State State { get; }

        public void Increment()
        {
            if (State.Total >= 10) throw new Exception("Too High");

            Uncommitted.Add(
                new TotalSet(State.Total + 1));
        }

        public void Decrement()
        {
            if (State.Total <= 0) throw new Exception("Too Low");

            Uncommitted.Add(
                new TotalSet(State.Total - 1));
        }

        public List<object> Uncommitted { get; } = new List<object>();
    }
}
