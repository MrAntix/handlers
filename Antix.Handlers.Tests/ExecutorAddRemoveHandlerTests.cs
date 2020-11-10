using Antix.Handlers.Tests.Model;
using Antix.Handlers.Tests.Model.Commands;
using Antix.Handlers.Tests.Model.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace Antix.Handlers.Tests
{
    [TestClass]
    public sealed class ExecutorAddRemoveHandlerTests
    {

        [TestMethod]
        public async Task multiple_handlers()
        {
            var serviceProvider = TestConfiguration.GetServiceProvider();

            var executor = serviceProvider.GetService<Executor<IEvent>>();
            var dataStore = serviceProvider.GetService<DataStore>();
            var eventLog = serviceProvider.GetService<EventLog>();

            await executor.ExecuteAsync(Event.From(new TotalSet(7), "One", 1));

            Assert.AreEqual(7, dataStore.Total);
            Assert.AreEqual(1, eventLog.GetAll().Count);
        }

        [TestMethod]
        public async Task remove_handlers()
        {
            var serviceProvider = TestConfiguration.GetServiceProvider();

            var executor = serviceProvider.GetService<Executor<IEvent>>()
                .RemoveHandlers<Event<TotalSet>>();
            var dataStore = serviceProvider.GetService<DataStore>();
            var eventLog = serviceProvider.GetService<EventLog>();

            await executor.ExecuteAsync(Event.From(new TotalSet(7), "One", 1));

            Assert.AreEqual(0, dataStore.Total);
            Assert.AreEqual(0, eventLog.GetAll().Count);
        }

        [TestMethod]
        public async Task handler_function()
        {
            var serviceProvider = TestConfiguration.GetServiceProvider();

            var are = new AutoResetEvent(false);
            var executor = serviceProvider.GetService<Executor<IEvent>>()
                .AddHandler<Event<TotalSet>>(d =>
                {
                    are.Set();
                });

            await executor.ExecuteAsync(Event.From(new TotalSet(7), "One", 1));
        }

        [TestMethod]
        public async Task handler_async_function()
        {
            var serviceProvider = TestConfiguration.GetServiceProvider();

            var are = new AutoResetEvent(false);
            var executor = serviceProvider.GetService<Executor<IEvent>>()
                .AddHandler<Event<TotalSet>>(d =>
                {
                    are.Set();
                    return Task.CompletedTask;
                });

            await executor.ExecuteAsync(Event.From(new TotalSet(7), "One", 1));
        }

        [TestMethod]
        public async Task scoped_handler_function()
        {
            var serviceProvider = TestConfiguration.GetServiceProvider();

            var are = new AutoResetEvent(false);
            var executor = serviceProvider.GetService<Executor<ICommandWrapper, Aggregate>>()
                .AddHandler<CommandWrapper<Increment>>((d, s) =>
                {
                    are.Set();
                });

            var data = CommandWrapper.From(new Increment(), "One", 1);

            await executor.ExecuteAsync(data, new Aggregate());
        }

        [TestMethod]
        public async Task scoped_handler_async_function()
        {
            var serviceProvider = TestConfiguration.GetServiceProvider();

            var are = new AutoResetEvent(false);
            var executor = serviceProvider.GetService<Executor<ICommandWrapper, Aggregate>>()
                .AddHandler<CommandWrapper<Increment>>((d, s) =>
                {
                    are.Set();
                    return Task.CompletedTask;
                });

            var data = CommandWrapper.From(new Increment(), "One", 1);

            await executor.ExecuteAsync(data, new Aggregate());
        }
    }
}
