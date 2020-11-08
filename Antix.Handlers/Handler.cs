using System;
using System.Threading.Tasks;

namespace Antix.Handlers
{
    public sealed class Handler<TMessage>
    {
        public Handler(
            Type messageType,
            Func<TMessage, Task> handleAsync)
        {
            MessageType = messageType;
            HandleAsync = handleAsync;
        }

        public Type MessageType { get; }
        public Func<TMessage, Task> HandleAsync { get; }
    }

    public sealed class Handler<TMessage, TScope>
        where TScope : class
    {
        public Handler(
            Type messageType,
            Type scopeType,
            Func<TMessage, TScope, Task> handleAsync)
        {
            MessageType = messageType;
            ScopeType = scopeType;
            HandleAsync = handleAsync;
        }

        public Type MessageType { get; }
        public Type ScopeType { get; }
        public Func<TMessage, TScope, Task> HandleAsync { get; }
    }
}
