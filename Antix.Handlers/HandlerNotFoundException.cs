using System;

namespace Antix.Handlers
{
    [Serializable]
    public class HandlerNotFoundException :
        Exception
    {
        public HandlerNotFoundException()
        {
        }

        protected HandlerNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) :
            base(info, context)
        { }
    }
}
