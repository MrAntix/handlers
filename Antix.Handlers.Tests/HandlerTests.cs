using Antix.Handlers.Tests.Model;
using Antix.Handlers.Tests.Model.Commands;
using Antix.Handlers.Tests.Model.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Antix.Handlers.Tests
{
    [TestClass]
    public sealed class HandlerTests
    {
        [TestMethod]
        public async Task successful_commands_update_manager_state()
        {
            var serviceProvider = GetServiceProvider();

            var manager = serviceProvider.GetService<Manager>();
            var subscriber = serviceProvider.GetService<Subscriber>();

            await RunEventSourcingScenarioAsync(manager, subscriber);

            Assert.AreEqual(6, manager.State.Total);
        }

        [TestMethod]
        public async Task successful_commands_update_eventStore()
        {
            var serviceProvider = GetServiceProvider();

            var manager = serviceProvider.GetService<Manager>();
            var subscriber = serviceProvider.GetService<Subscriber>();

            await RunEventSourcingScenarioAsync(manager, subscriber);

            Assert.AreEqual(3, manager.EventsStore.Count);
            Assert.AreEqual(4U, manager.EventsStore[2].SequenceNumber);
        }

        [TestMethod]
        public async Task successful_event_handlers_update_dataStore()
        {
            var serviceProvider = GetServiceProvider();

            var manager = serviceProvider.GetService<Manager>();
            var subscriber = serviceProvider.GetService<Subscriber>();
            var dataStore = serviceProvider.GetService<DataStore>();

            await RunEventSourcingScenarioAsync(manager, subscriber);

            Assert.AreEqual(6, dataStore.Total);
        }

        static async Task RunEventSourcingScenarioAsync(
            Manager manager, Subscriber subscriber)
        {
            manager.LoadState(new[]{
                Event.From(new TotalSet(5), "Initial", 1 )
            });

            using (manager.Events.Subscribe(subscriber))
            {

                await manager.ExecuteAsync(new Increment(), "One");
                await manager.ExecuteAsync(new Increment(), "Two");
                await manager.ExecuteAsync(new Decrement(), "Three");
            }
        }

        [TestMethod]
        public async Task cause_error()
        {
            var serviceProvider = GetServiceProvider();

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

        static ServiceProvider GetServiceProvider()
        {
            return new ServiceCollection()
                .AddTransient<Manager>()
                .AddTransient<Subscriber>()
                .AddSingleton<DataStore>()
                .AddHandlers<ICommandWrapper, Aggregate>()
                .AddHandlers<IEvent>()
                .BuildServiceProvider();
        }
    }
}
