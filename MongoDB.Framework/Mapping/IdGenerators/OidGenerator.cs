using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DriverOidGenerator = MongoDB.Driver.OidGenerator;

namespace MongoDB.Framework.Mapping.IdGenerators
{
    public class OidGenerator : IIdGenerator
    {
        public object Generate(object entity, IMongoContext mongoContext)
        {
            DriverOidGenerator gen = new DriverOidGenerator();
            return BitConverter.ToString(gen.Generate().Value).Replace("-", "").ToLower();
        }
    }
}