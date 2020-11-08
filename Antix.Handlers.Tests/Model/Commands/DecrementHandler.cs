using System.Threading.Tasks;

namespace Antix.Handlers.Tests.Model.Commands
{
    public sealed class DecrementHandler :
        IHandler<CommandWrapper<Decrement>, Aggregate>
    {
        Task IHandler<CommandWrapper<Decrement>, Aggregate>.HandleAsync(
            CommandWrapper<Decrement> _,
            Aggregate aggregate
            )
        {
            aggregate.Decrement();

            return Task.CompletedTask;
        }
    }
}
