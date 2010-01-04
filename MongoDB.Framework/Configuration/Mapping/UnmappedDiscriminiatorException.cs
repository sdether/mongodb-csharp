using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping
{
    [global::System.Serializable]
    public class UnmappedDiscriminatorException : Exception
    {
        public UnmappedDiscriminatorException(string message) : base(message) { }
        protected UnmappedDiscriminatorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
