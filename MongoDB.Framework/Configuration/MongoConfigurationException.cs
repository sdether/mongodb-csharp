using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration
{
    [global::System.Serializable]
    public class MongoConfigurationException : Exception
    {
        public MongoConfigurationException(string message) : base(message) { }
        protected MongoConfigurationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
