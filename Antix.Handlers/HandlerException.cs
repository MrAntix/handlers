using System;
using System.Reflection;

namespace Antix.Handlers
{
    [Serializable]
    public class HandlerException<TDataImplements> : 
        Exception
    {
        public HandlerException(
            TDataImplements attempted,
            TargetInvocationException inner) :
            base(inner.InnerException.Message, inner)
        {
            Attempted = attempted;
        }

        protected HandlerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) :
            base(info, context)
        { }

        public TDataImplements Attempted { get; }
    }

    [Serializable]
    public class HandlerException<TDataImplements, TScope> : 
        Exception
    {
        public HandlerException(
            TDataImplements attempted,
            TScope scope,
            TargetInvocationException inner) :
            base(inner.InnerException.Message, inner)
        {
            Attempted = attempted;
            Scope = scope;
        }

        protected HandlerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) :
            base(info, context)
        { }

        public TDataImplements Attempted { get; }
        public TScope Scope { get; }
    }
}
