using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Antix.Handlers.Tests.Model
{
    public sealed class Manager
    {
        readonly Executor<ICommandWrapper, Aggregate> _commandHandlers;
        readonly Subject<IEvent> _events;
        readonly IList<IEvent> _eventsStore;


        uint _commandSequenceNumber;

        public Manager(
            Executor<ICommandWrapper, Aggregate> commandHandlers)
        {
            _commandHandlers = commandHandlers;
            _events = new Subject<IEvent>();
            _eventsStore = new List<IEvent>();


            State = new State();
        }

        public State State { get; private set; }

        public void LoadState(IEnumerable<IEvent> events)
        {
            State = new State()
                .Apply(events.ToArray());
        }

        public IObservable<IEvent> Events => _events;
        public IImmutableList<IEvent> EventsStore => _eventsStore.ToImmutableList();

        public async Task ExecuteAsync<TCommand>(TCommand command, string userId)
        {
            var aggregate = new Aggregate(State);

            var commandSequenceNumber = ++_commandSequenceNumber;
            await _commandHandlers.ExecuteAsync(
                new CommandWrapper<TCommand>(command, userId, commandSequenceNumber),
                aggregate
                );

            var events = WrapEventData(aggregate.Uncommitted, userId);

            foreach (var e in events)
            {
                State = State.Apply(e);
                _eventsStore.Add(e);
                _events.OnNext(e);
            }
        }

        IEvent[] WrapEventData(
            IEnumerable<object> datas, string userId)
        {
            var version = State?.Version ?? 0;

            return datas
                .Select(data => Event.From(data, userId, ++version))
                .ToArray();
        }
    }
}
