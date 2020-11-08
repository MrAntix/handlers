using System.Threading.Tasks;

namespace Antix.Handlers.Tests.Model.Events
{
    public sealed class TotalSetHandler :
        IHandler<Event<TotalSet>>
    {
        readonly Store _store;

        public TotalSetHandler(
            Store store)
        {
            _store = store;
        }

        public Task HandleAsync(
            Event<TotalSet> e)
        {
            _store.Total = e.Data.Value;

            return Task.CompletedTask;
        }
    }
}
