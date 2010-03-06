using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Proxy
{
    [global::System.Serializable]
    public class LazyInitializationException : Exception
    {
        public LazyInitializationException(string message) : base(message) { }
        protected LazyInitializationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
