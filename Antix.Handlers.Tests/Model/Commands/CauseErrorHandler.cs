using System;
using System.Threading.Tasks;

namespace Antix.Handlers.Tests.Model.Commands
{
    public sealed class CauseErrorHandler :
        IHandler<CommandWrapper<CauseError>, Aggregate>
    {
        Task IHandler<CommandWrapper<CauseError>, Aggregate>
            .HandleAsync(CommandWrapper<CauseError> message, Aggregate scope)
        {
            throw new Exception(message.Command.Text);
        }
    }
}
