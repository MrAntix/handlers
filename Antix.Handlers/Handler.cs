using System;
using System.Threading.Tasks;

namespace Antix.Handlers
{
    public sealed class Handler<TData>
    {
        public Handler(
            Type dataType,
            Func<TData, Task> handleAsync)
        {
            DataType = dataType;
            HandleAsync = handleAsync;
        }

        public Type DataType { get; }
        public Func<TData, Task> HandleAsync { get; }
    }

    public sealed class Handler<TData, TScope>
        where TScope : class
    {
        public Handler(
            Type dataType,
            Type scopeType,
            Func<TData, TScope, Task> handleAsync)
        {
            DataType = dataType;
            ScopeType = scopeType;
            HandleAsync = handleAsync;
        }

        public Type DataType { get; }
        public Type ScopeType { get; }
        public Func<TData, TScope, Task> HandleAsync { get; }
    }
}
