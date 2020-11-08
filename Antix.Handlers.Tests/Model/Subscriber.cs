using Antix.Handlers.Tests.Model.Events;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reactive;

namespace Antix.Handlers.Tests.Model
{
    public sealed class Subscriber : ObserverBase<IEvent>
    {
        readonly Executor<IEvent> _executor;
        readonly IList<IEvent> _events;

        public Subscriber(
            Executor<IEvent> executor)
        {
            _executor = executor;
            _events = new List<IEvent>();
        }

        public IImmutableList<IEvent> Events => _events.ToImmutableList();

        protected override void OnNextCore(IEvent e)
        {
            _executor.Execute(e);
            _events.Add(e);
        }

        protected override void OnCompletedCore() { }

        protected override void OnErrorCore(Exception error) { }
    }
}
