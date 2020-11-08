using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antix.Handlers
{
    /// <summary>
    /// Executor for handlers for the given message type
    /// </summary>
    /// <typeparam name="TMessage">Message Type</typeparam>
    public sealed class Executor<TMessage>
    {
        readonly Dictionary<Type, Func<TMessage, Task>> _handlers;

        public Executor(
            IEnumerable<Handler<TMessage>> handlers)
        {
            _handlers = handlers.ToDictionary(
                handler => handler.MessageType,
                handler => handler.HandleAsync
                );
        }

        /// <summary>
        /// Execute the handler for the message passed
        /// </summary>
        /// <typeparam name="T">Actual type of the message, implements TMessage</typeparam>
        /// <param name="message">Message</param>
        public Task ExecuteAsync<T>(
            T message)
            where T : TMessage
        {
            return _handlers[message.GetType()](message);
        }

        /// <summary>
        /// Execute the handler sync for the message passed
        /// </summary>
        /// <typeparam name="T">Actual type of the message, implements TMessage</typeparam>
        /// <param name="message">Message</param>
        public void Execute<T>(
            T message)
            where T : TMessage
        {
            _handlers[message.GetType()](message).GetAwaiter().GetResult();
        }
    }

    /// <summary>
    /// Executor for handlers for the given message type and scope
    /// </summary>
    /// <typeparam name="TMessage">Message Type</typeparam>
    /// <typeparam name="TScope">TScope</typeparam>
    public sealed class Executor<TMessage, TScope>
        where TScope : class
    {
        readonly Dictionary<Type, Func<TMessage, TScope, Task>> _handlers;

        public Executor(
            IEnumerable<Handler<TMessage, TScope>> handlers)
        {
            _handlers = handlers.ToDictionary(
                handler => handler.MessageType,
                handler => handler.HandleAsync
                );
        }

        /// <summary>
        /// Execute the handler for the message and scope passed
        /// </summary>
        /// <typeparam name="T">Actual type of the message, implements TMessage</typeparam>
        /// <param name="message">Message</param>
        /// <param name="scope">Scope</param>
        public Task ExecuteAsync<T>(
            T message,
            TScope scope)
            where T : TMessage
        {
            return _handlers[message.GetType()](message, scope);
        }

        /// <summary>
        /// Execute the handler for the message and scope passed
        /// </summary>
        /// <typeparam name="T">Actual type of the message, implements TMessage</typeparam>
        /// <param name="message">Message</param>
        /// <param name="scope">Scope</param>
        public void Execute<T>(
            T message,
            TScope scope)
            where T : TMessage
        {
            _handlers[message.GetType()](message, scope).GetAwaiter().GetResult();
        }
    }

}
