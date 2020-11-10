using Antix.Handlers.Tests.Model;
using Antix.Handlers.Tests.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Antix.Handlers.Tests
{
    [TestClass]
    public sealed class ErrorHandlingTests
    {
        [TestMethod]
        public async Task cause_error()
        {
            var serviceProvider = TestConfiguration.GetServiceProvider();

            var manager = serviceProvider.GetService<Manager>();

            const string errorText = "ERROR";
            var command = new CauseError(errorText);
            var ex = await Assert
                .ThrowsExceptionAsync<HandlerException<ICommandWrapper, Aggregate>>(
                    async () =>
                        await manager.ExecuteAsync(command, "One")
                        );

            Assert.AreEqual(command, ex.Attempted.Command);
        }

    }
}
