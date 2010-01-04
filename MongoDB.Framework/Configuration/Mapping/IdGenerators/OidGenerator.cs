using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DriverOidGenerator = MongoDB.Driver.OidGenerator;

namespace MongoDB.Framework.Configuration.Mapping.IdGenerators
{
    public class OidGenerator : IIdGenerator
    {
        public object Generate(object entity, IMongoContextImplementor mongoContext)
        {
            DriverOidGenerator gen = new DriverOidGenerator();
            return BitConverter.ToString(gen.Generate().Value).Replace("-", "").ToLower();
        }
    }
}