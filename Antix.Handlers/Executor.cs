using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Antix.Handlers
{
    /// <summary>
    /// Executor for handlers for the given data type
    /// </summary>
    /// <typeparam name="TDataImplements">Implemented by all data</typeparam>
    public sealed class Executor<TDataImplements>
    {
        readonly IImmutableList<Handler<TDataImplements>> _allHandlers;
        readonly IImmutableDictionary<Type, ImmutableList<Func<TDataImplements, Task>>> _handlers;

        readonly ExecutorOptions _options;

        public Executor(
            IEnumerable<Handler<TDataImplements>> handlers = null,
            ExecutorOptions options = null)
        {
            _allHandlers = handlers?.ToImmutableList()
                ?? ImmutableList<Handler<TDataImplements>>.Empty;

            _handlers = _allHandlers.Aggregate(
                ImmutableDictionary<Type, ImmutableList<Func<TDataImplements, Task>>>.Empty,
                (result, handler) =>
                {
                    if (!result.ContainsKey(handler.DataType))
                        return result.Add(
                            handler.DataType,
                            ImmutableList<Func<TDataImplements, Task>>.Empty.Add(handler.HandleAsync)
                            );


                    return result.SetItem(
                        handler.DataType,
                        result[handler.DataType].Add(handler.HandleAsync)
                        );
                });

            DataTypes = _handlers.Keys.ToImmutableList();

            _options = options ?? ExecutorOptions.Default;
        }

        /// <summary>
        /// All handled data types
        /// </summary>
        public IImmutableList<Type> DataTypes { get; }

        /// <summary>
        /// Add a delegate/method as a handler
        /// </summary>
        /// <typeparam name="TData">Type of data</typeparam>
        /// <param name="handle">Handle delegate/method</param>
        /// <returns>A new Executor</returns>
        public Executor<TDataImplements> AddHandler<TData>(Action<TData> handle)
            where TData : class, TDataImplements
        {
            return new Executor<TDataImplements>(
                    _allHandlers.Add(Handler<TDataImplements>.From(handle))
                );
        }

        /// <summary>
        /// Add a delegate/method as a handler
        /// </summary>
        /// <typeparam name="TData">Type of data</typeparam>
        /// <param name="handle">Handle delegate/method</param>
        /// <returns>A new Executor</returns>
        public Executor<TDataImplements> AddHandler<TData>(Func<TData, Task> handle)
            where TData : class, TDataImplements
        {
            return new Executor<TDataImplements>(
                    _allHandlers.Add(Handler<TDataImplements>.From(handle))
                );
        }

        /// <summary>
        /// Add a handler
        /// </summary>
        /// <typeparam name="TData">Type of data</typeparam>
        /// <param name="handler">A handler</param>
        /// <returns>A new Executor</returns>
        public Executor<TDataImplements> AddHandler<TData>(IHandler<TData> handler)
            where TData : class, TDataImplements
        {
            return new Executor<TDataImplements>(
                    _allHandlers.Add(Handler<TDataImplements>.From(handler))
                );
        }

        /// <summary>
        /// Remove all handlers for a given type
        /// </summary>
        /// <typeparam name="TData">Type of data</typeparam>
        /// <returns>A new Executor</returns>
        public Executor<TDataImplements> RemoveHandlers<TData>()
            where TData : TDataImplements
        {
            return new Executor<TDataImplements>(
                    _allHandlers.RemoveAll(h => h.DataType == typeof(TData))
                );
        }

        /// <summary>
        /// Execute the handler for the data passed
        /// </summary>
        /// <typeparam name="T">Actual type of the data, implements TData</typeparam>
        /// <param name="data">Data</param>
        public Task ExecuteAsync<T>(
            T data)
            where T : TDataImplements
        {
            var dataType = data.GetType();
            var handlers = _handlers.ContainsKey(dataType)
                ? _handlers[dataType]
                : null;
            if (handlers == null)
            {
                if (!_options.RequireHandlers) return Task.CompletedTask;

                throw new HandlerNotFoundException();
            }

            return Task.WhenAll(
                handlers
                    .Select(handler => handler(data))
                    .ToArray()
                );
        }

        /// <summary>
        /// Execute the handler sync for the data passed
        /// </summary>
        /// <typeparam name="T">Actual type of the data, implements TData</typeparam>
        /// <param name="data">Data</param>
        public void Execute<T>(
            T data)
            where T : TDataImplements
        {
            ExecuteAsync(data).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Make handlers required, will throw HandlerNotFoundException
        /// </summary>
        /// <returns>Executor</returns>
        public Executor<TDataImplements> RequireHandlers()
        {
            return new Executor<TDataImplements>(
                _allHandlers,
                _options.SetRequireHandlers(true)
                );
        }
    }

    /// <summary>
    /// Executor for handlers for the given data type and scope
    /// </summary>
    /// <typeparam name="TDataImplements">Implemented by all data</typeparam>
    /// <typeparam name="TScope">TScope</typeparam>
    public sealed class Executor<TDataImplements, TScope>
        where TScope : class
    {
        readonly IImmutableList<Handler<TDataImplements, TScope>> _allHandlers;
        readonly IImmutableDictionary<Type, ImmutableList<Func<TDataImplements, TScope, Task>>> _handlers;

        readonly ExecutorOptions _options;

        public Executor(
            IEnumerable<Handler<TDataImplements, TScope>> handlers = null,
            ExecutorOptions options = null)
        {
            _allHandlers = handlers?.ToImmutableList()
                ?? ImmutableList<Handler<TDataImplements, TScope>>.Empty;

            _handlers = _allHandlers.Aggregate(
                ImmutableDictionary<Type, ImmutableList<Func<TDataImplements, TScope, Task>>>.Empty,
                (result, handler) =>
                {
                    if (!result.ContainsKey(handler.DataType))
                        return result.Add(
                            handler.DataType,
                            ImmutableList<Func<TDataImplements, TScope, Task>>.Empty.Add(handler.HandleAsync)
                            );


                    return result.SetItem(
                        handler.DataType,
                        result[handler.DataType].Add(handler.HandleAsync)
                        );
                });

            DataTypes = _handlers.Keys.ToImmutableList();

            _options = options ?? ExecutorOptions.Default;
        }

        /// <summary>
        /// All handled data types
        /// </summary>
        public IImmutableList<Type> DataTypes { get; }

        /// <summary>
        /// Add a delegate/method as a handler
        /// </summary>
        /// <typeparam name="TData">Type of data</typeparam>
        /// <param name="handle">Handle delegate/method</param>
        /// <returns>A new Executor</returns>
        public Executor<TDataImplements, TScope> AddHandler<TData>(Action<TData, TScope> handle)
            where TData : class, TDataImplements
        {
            return new Executor<TDataImplements, TScope>(
                    _allHandlers.Add(Handler<TDataImplements, TScope>.From(handle))
                );
        }

        /// <summary>
        /// Add a delegate/method as a handler
        /// </summary>
        /// <typeparam name="TData">Type of data</typeparam>
        /// <param name="handle">Handle delegate/method</param>
        /// <returns>A new Executor</returns>
        public Executor<TDataImplements, TScope> AddHandler<TData>(Func<TData, TScope, Task> handle)
            where TData : class, TDataImplements
        {
            return new Executor<TDataImplements, TScope>(
                    _allHandlers.Add(Handler<TDataImplements, TScope>.From(handle))
                );
        }

        /// <summary>
        /// Add a handler
        /// </summary>
        /// <typeparam name="TData">Type of data</typeparam>
        /// <param name="handler">A handler</param>
        /// <returns>A new Executor</returns>
        public Executor<TDataImplements, TScope> AddHandler<TData>(IHandler<TData, TScope> handler)
            where TData : class, TDataImplements
        {
            return new Executor<TDataImplements, TScope>(
                    _allHandlers.Add(Handler<TDataImplements, TScope>.From(handler))
                );
        }

        /// <summary>
        /// Remove all handlers for a given type
        /// </summary>
        /// <typeparam name="TData">Type of data</typeparam>
        /// <returns>A new Executor</returns>
        public Executor<TDataImplements, TScope> RemoveHandlers<TData>()
            where TData : TDataImplements
        {
            return new Executor<TDataImplements, TScope>(
                    _allHandlers.RemoveAll(h => h.DataType == typeof(TData))
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
            where T : TDataImplements
        {
            var dataType = data.GetType();
            var handlers = _handlers.ContainsKey(dataType)
                ? _handlers[dataType]
                : null;
            if (handlers == null)
            {
                if (!_options.RequireHandlers) return Task.CompletedTask;

                throw new HandlerNotFoundException();
            }

            return Task.WhenAll(
                handlers
                    .Select(handler => handler(data, scope))
                    .ToArray()
                );
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
            where T : TDataImplements
        {
            ExecuteAsync(data, scope).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Make handlers required, will throw HandlerNotFoundException
        /// </summary>
        /// <returns>Executor</returns>
        public Executor<TDataImplements, TScope> RequireHandlers()
        {
            return new Executor<TDataImplements, TScope>(
                _allHandlers,
                _options.SetRequireHandlers(true)
                ); ;
        }
    }
}
