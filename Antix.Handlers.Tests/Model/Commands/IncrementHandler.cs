using System.Threading.Tasks;

namespace Antix.Handlers.Tests.Model.Commands
{
    public sealed class IncrementHandler :
        IHandler< CommandWrapper<Increment>,Aggregate>
    {
        Task IHandler< CommandWrapper<Increment>,Aggregate>.HandleAsync(
            CommandWrapper<Increment> wrapper,
            Aggregate aggregate)
        {
            aggregate.Increment();

            return Task.CompletedTask;
        }
    }
}
