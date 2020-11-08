using Antix.Handlers.Tests.Model.Events;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reactive;

namespace Antix.Handlers.Tests.Model
{
    public sealed class Subscriber : ObserverBase<IEvent>
    {
        readonly Executor<IEvent> _eventHandlers;
        readonly IList<IEvent> _events;

        public Subscriber(
            Executor<IEvent> eventHandlers)
        {
            _eventHandlers = eventHandlers;
            _events = new List<IEvent>();
        }

        public IImmutableList<IEvent> Events => _events.ToImmutableList();

        protected override void OnNextCore(IEvent e)
        {
            _eventHandlers.Execute(e);
            _events.Add(e);
        }

        protected override void OnCompletedCore() { }

        protected override void OnErrorCore(Exception error) { }
    }
}
