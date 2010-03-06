using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Mapping.IdGenerators
{
    [global::System.Serializable]
    public class IdGenerationException : Exception
    {
        public IdGenerationException(string message) : base(message) { }
        protected IdGenerationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
