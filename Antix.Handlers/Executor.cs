﻿using System;
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
        readonly IEnumerable<Handler<TData>> _allHandlers;
        readonly Dictionary<Type, Func<TData, Task>> _handlers;
        readonly ExecutorOptions _options;

        public Executor(
            IEnumerable<Handler<TData>> handlers,
            ExecutorOptions options = null)
        {
            _allHandlers = handlers;
            _handlers = handlers.ToDictionary(
                handler => handler.DataType,
                handler => handler.HandleAsync
                );
            _options = options ?? ExecutorOptions.Default;
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
            var handler = _handlers.ContainsKey(data.GetType())
                ? _handlers[data.GetType()]
                : null;
            if (handler == null)
            {
                if (!_options.RequireHandlers) return Task.CompletedTask;

                throw new HandlerNotFoundException();
            }
            return handler(data);
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
            ExecuteAsync(data).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Make handlers required, will throw HandlerNotFoundException
        /// </summary>
        /// <returns>Executor</returns>
        public Executor<TData> RequireHandlers()
        {
            return new Executor<TData>(
                _allHandlers,
                _options.SetRequireHandlers(true)
                ); ;
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
        readonly IEnumerable<Handler<TData, TScope>> _allHandlers;
        readonly Dictionary<Type, Func<TData, TScope, Task>> _handlers;
        readonly ExecutorOptions _options;

        public Executor(
            IEnumerable<Handler<TData, TScope>> handlers,
            ExecutorOptions options = null)
        {
            _allHandlers = handlers;
            _handlers = handlers.ToDictionary(
                handler => handler.DataType,
                handler => handler.HandleAsync
                );
            _options = options ?? ExecutorOptions.Default;
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
            var handler = _handlers.ContainsKey(data.GetType())
                ? _handlers[data.GetType()]
                : null;
            if (handler == null)
            {
                if (!_options.RequireHandlers) return Task.CompletedTask;

                throw new HandlerNotFoundException();
            }

            return handler(data, scope);
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
            ExecuteAsync(data, scope).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Make handlers required, will throw HandlerNotFoundException
        /// </summary>
        /// <returns>Executor</returns>
        public Executor<TData, TScope> RequireHandlers()
        {
            return new Executor<TData, TScope>(
                _allHandlers,
                _options.SetRequireHandlers(true)
                ); ;
        }
    }
}
