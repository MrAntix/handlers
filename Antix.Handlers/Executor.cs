using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antix.Handlers
{
    /// <summary>
    /// Executor for handlers for the given data type
    /// </summary>
    /// <typeparam name="TData">Data Type</typeparam>
    public sealed class Executor<TData>
    {
        readonly Dictionary<Type, Func<TData, Task>> _handlers;

        public Executor(
            IEnumerable<Handler<TData>> handlers)
        {
            _handlers = handlers.ToDictionary(
                handler => handler.DataType,
                handler => handler.HandleAsync
                );
        }

        /// <summary>
        /// Execute the handler for the data passed
        /// </summary>
        /// <typeparam name="T">Actual type of the data, implements TData</typeparam>
        /// <param name="data">Data</param>
        public Task ExecuteAsync<T>(
            T data)
            where T : TData
        {
            return _handlers[data.GetType()](data);
        }

        /// <summary>
        /// Execute the handler sync for the data passed
        /// </summary>
        /// <typeparam name="T">Actual type of the data, implements TData</typeparam>
        /// <param name="data">Data</param>
        public void Execute<T>(
            T data)
            where T : TData
        {
            _handlers[data.GetType()](data).GetAwaiter().GetResult();
        }
    }

    /// <summary>
    /// Executor for handlers for the given data type and scope
    /// </summary>
    /// <typeparam name="TData">Data Type</typeparam>
    /// <typeparam name="TScope">TScope</typeparam>
    public sealed class Executor<TData, TScope>
        where TScope : class
    {
        readonly Dictionary<Type, Func<TData, TScope, Task>> _handlers;

        public Executor(
            IEnumerable<Handler<TData, TScope>> handlers)
        {
            _handlers = handlers.ToDictionary(
                handler => handler.DataType,
                handler => handler.HandleAsync
                );
        }

        /// <summary>
        /// Execute the handler for the data and scope passed
        /// </summary>
        /// <typeparam name="T">Actual type of the data, implements TData</typeparam>
        /// <param name="data">Data</param>
        /// <param name="scope">Scope</param>
        public Task ExecuteAsync<T>(
            T data,
            TScope scope)
            where T : TData
        {
            return _handlers[data.GetType()](data, scope);
        }

        /// <summary>
        /// Execute the handler for the data and scope passed
        /// </summary>
        /// <typeparam name="T">Actual type of the data, implements TData</typeparam>
        /// <param name="data">Data</param>
        /// <param name="scope">Scope</param>
        public void Execute<T>(
            T data,
            TScope scope)
            where T : TData
        {
            _handlers[data.GetType()](data, scope).GetAwaiter().GetResult();
        }
    }

}
