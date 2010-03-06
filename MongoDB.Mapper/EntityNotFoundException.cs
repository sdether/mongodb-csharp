using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Mapper.Configuration;
using MongoDB.Mapper.Linq;
using MongoDB.Mapper.Configuration.Mapping;
using MongoDB.Mapper.Persistence;
using MongoDB.Mapper.Tracking;

namespace MongoDB.Mapper
{
    [global::System.Serializable]
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message) { }
        protected EntityNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
