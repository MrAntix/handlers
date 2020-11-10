using Antix.Handlers.Tests.Model;
using Antix.Handlers.Tests.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Antix.Handlers.Tests
{
    [TestClass]
    public sealed class ExecutorOptionsTests
    {
        [TestMethod]
        public async Task ignore_when_no_handler()
        {
            var serviceProvider = TestConfiguration.GetServiceProvider();

            var manager = serviceProvider.GetService<Manager>();

            var command = new NoHandler();
            await manager.ExecuteAsync(command, "One");
        }

        [TestMethod]
        public async Task require_handlers()
        {
            var serviceProvider = TestConfiguration.GetServiceProvider(
                ExecutorOptions.Default.SetRequireHandlers(true)
                );

            var manager = serviceProvider.GetService<Manager>();

            var command = new NoHandler();
            var ex = await Assert
                .ThrowsExceptionAsync<HandlerNotFoundException>(
                    async () =>
                        await manager.ExecuteAsync(command, "One")
                        );
        }
    }
}
