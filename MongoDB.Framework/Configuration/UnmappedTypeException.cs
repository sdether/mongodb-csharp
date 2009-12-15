using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration
{
    [global::System.Serializable]
    public class UnmappedTypeException : Exception
    {
        public UnmappedTypeException(string message) : base(message) { }
        protected UnmappedTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
