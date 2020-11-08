using Antix.Handlers.Tests.Model;
using Antix.Handlers.Tests.Model.Commands;
using Antix.Handlers.Tests.Model.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Antix.Handlers.Tests
{
    [TestClass]
    public sealed class HandlerTests
    {
        [TestMethod]
        public async Task handlers_called()
        {
            var serviceProvider = GetServiceProvider();

            var manager = serviceProvider.GetService<Manager>();
            manager.LoadState(new[]{
                Event.From(new TotalSet(5), "Initial", 1 )
            });

            var subscriber = serviceProvider.GetService<Subscriber>();
            manager.Events.Subscribe(subscriber);

            var store = serviceProvider.GetService<Store>();

            await manager.ExecuteAsync(new Increment(), "One");
            await manager.ExecuteAsync(new Increment(), "Two");
            await manager.ExecuteAsync(new Decrement(), "Three");

            Assert.AreEqual(6, manager.State.Total);
            Assert.AreEqual(6, store.Total);
            Assert.AreEqual(3, subscriber.Events.Count);
            Assert.AreEqual(4U, subscriber.Events[2].SequenceNumber);
        }

        [TestMethod]
        public async Task cause_error()
        {
            var serviceProvider = GetServiceProvider();

            var manager = serviceProvider.GetService<Manager>();

            const string errorText = "ERROR";
            var ex = await Assert.ThrowsExceptionAsync<Exception>(async () =>
               await manager.ExecuteAsync(new CauseError(errorText), "One")
             );

            Assert.AreEqual(errorText, ex.Message);
        }

        static ServiceProvider GetServiceProvider()
        {
            return new ServiceCollection()
                .AddTransient<Manager>()
                .AddTransient<Subscriber>()
                .AddSingleton<Store>()
                .AddHandlers<ICommandWrapper, Aggregate>()
                .AddHandlers<IEvent>()
                .BuildServiceProvider();
        }
    }
}
