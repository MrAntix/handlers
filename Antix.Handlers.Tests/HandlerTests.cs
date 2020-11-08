using Antix.Handlers.Tests.Model;
using Antix.Handlers.Tests.Model.Commands;
using Antix.Handlers.Tests.Model.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Antix.Handlers.Tests
{
    [TestClass]
    public sealed class HandlerTests
    {
        [TestMethod]
        public void handlers_called()
        {
            var serviceProvider = new ServiceCollection()
                .AddTransient<Manager>()
                .AddTransient<Subscriber>()
                .AddSingleton<Store>()
                .AddHandlers<ICommandWrapper, Aggregate>()
                .AddHandlers<IEvent>()
                .BuildServiceProvider();

            var manager = serviceProvider.GetService<Manager>();
            manager.LoadState(new[]{
                Event.From(new TotalSet(5), "Initial", 1 )
            });

            var subscriber = serviceProvider.GetService<Subscriber>();
            manager.Events.Subscribe(subscriber);

            var store = serviceProvider.GetService<Store>();

            manager.Execute(new Increment(), "One");
            manager.Execute(new Increment(), "Two");
            manager.Execute(new Decrement(), "Three");

            Assert.AreEqual(6, manager.State.Total);
            Assert.AreEqual(6, store.Total);
            Assert.AreEqual(3, subscriber.Events.Count);
            Assert.AreEqual(4U, subscriber.Events[2].SequenceNumber);
        }
    }
}
