using System;
using System.Threading.Tasks;

namespace Antix.Handlers
{
    public sealed class Handler<TDataImplements>
    {
        public Handler(
            Type dataType,
            Func<TDataImplements, Task> handleAsync)
        {
            DataType = dataType;
            HandleAsync = handleAsync;
        }

        public Type DataType { get; }
        public Func<TDataImplements, Task> HandleAsync { get; }

        public static Handler<TDataImplements> From<TData>(
            IHandler<TData> handler)
            where TData : class, TDataImplements
        {
            return new Handler<TDataImplements>(
                typeof(TData),
                data =>
                {
                    return handler.HandleAsync((TData)data);
                });
        }

        public static Handler<TDataImplements> From<TData>(
            Action<TData> action)
            where TData : TDataImplements
        {
            return new Handler<TDataImplements>(
                typeof(TData),
                data =>
                {
                    action((TData)data);
                    return Task.CompletedTask;
                });
        }

        public static Handler<TDataImplements> From<TData>(
            Func<TData, Task> action)
            where TData : TDataImplements
        {
            return new Handler<TDataImplements>(
                typeof(TData),
                data =>
                {
                    return action((TData)data);
                });
        }
    }

    public sealed class Handler<TDataImplements, TScope>
        where TScope : class
    {
        public Handler(
            Type dataType,
            Func<TDataImplements, TScope, Task> handleAsync)
        {
            DataType = dataType;
            HandleAsync = handleAsync;
        }

        public Type DataType { get; }
        public Type ScopeType { get; } = typeof(TScope);
        public Func<TDataImplements, TScope, Task> HandleAsync { get; }

        public static Handler<TDataImplements, TScope> From<TData>(
            IHandler<TData, TScope> handler)
            where TData : class, TDataImplements
        {
            return new Handler<TDataImplements, TScope>(
                typeof(TData),
                (data, scope) =>
                {
                    return handler.HandleAsync((TData)data, scope);
                });
        }

        public static Handler<TDataImplements, TScope> From<TData>(
            Action<TData, TScope> action)
            where TData : TDataImplements
        {
            return new Handler<TDataImplements, TScope>(
                typeof(TData),
                (data, scope) =>
                {
                    action((TData)data, scope);
                    return Task.CompletedTask;
                });
        }

        public static Handler<TDataImplements, TScope> From<TData>(
            Func<TData, TScope, Task> action)
            where TData : TDataImplements
        {
            return new Handler<TDataImplements, TScope>(
                typeof(TData),
                (data, scope) =>
                {
                    return action((TData)data, scope);
                });
        }
    }
}
