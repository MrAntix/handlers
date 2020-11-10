using System.Threading.Tasks;

namespace Antix.Handlers.Tests.Model.Events
{
    public sealed class LogTotalSetHandler :
        IHandler<Event<TotalSet>>
    {
        readonly EventLog _log;

        public LogTotalSetHandler(
            EventLog log)
        {
            _log = log;
        }

        public Task HandleAsync(
            Event<TotalSet> e)
        {
            _log.Add($"Set Total to `{e.Data.Value}`");

            return Task.CompletedTask;
        }
    }
}
