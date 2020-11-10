using Antix.Handlers.Tests.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Antix.Handlers.Tests
{
    public static class TestConfiguration
    {
        public static ServiceProvider GetServiceProvider(
            ExecutorOptions executorOptions = null)
        {
            return new ServiceCollection()
                .AddSingleton(executorOptions ?? ExecutorOptions.Default)
                .AddSingleton<Manager>()
                .AddSingleton<Subscriber>()
                .AddSingleton<DataStore>()
                .AddSingleton<EventLog>()
                .AddHandlers<ICommandWrapper, Aggregate>()
                .AddHandlers<IEvent>()
                .BuildServiceProvider();
        }
    }
}
