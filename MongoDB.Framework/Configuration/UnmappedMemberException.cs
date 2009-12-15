using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration
{
    [global::System.Serializable]
    public class UnmappedMemberException : Exception
    {
        public UnmappedMemberException(string message) : base(message) { }
        protected UnmappedMemberException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
