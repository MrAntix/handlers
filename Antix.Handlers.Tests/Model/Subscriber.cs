using System;

namespace Antix.Handlers.Tests.Model
{
    public sealed class Subscriber : IObserver<IEvent>
    {
        readonly Executor<IEvent> _executor;

        public Subscriber(
            Executor<IEvent> executor)
        {
            _executor = executor;
        }

        public void OnNext(IEvent e)
        {
            _executor.Execute(e);
        }

        public void OnError(Exception ex)
        {
            throw ex;
        }

        public void OnCompleted()
        {
        }
    }
}
