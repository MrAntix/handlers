using Antix.Handlers.Tests.Model;
using Antix.Handlers.Tests.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Antix.Handlers.Tests
{
    [TestClass]
    public sealed class ExecutorPropertiesTests
    {
        [TestMethod]
        public void exposes_data_types()
        {
            var serviceProvider = TestConfiguration.GetServiceProvider();

            var executor = serviceProvider
                .GetService<Executor<ICommandWrapper, Aggregate>>();

            CollectionAssert.AreEquivalent(
                new[] {
                    typeof(CommandWrapper<CauseError>),
                    typeof(CommandWrapper<Decrement>),
                    typeof(CommandWrapper<Increment>)
                },
                executor.DataTypes.ToArray()
                );
        }
    }
}
