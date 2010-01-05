using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.IdGenerators
{
    public class GuidGenerator : IIdGenerator
    {
        /// <summary>
        /// Generates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="mongoSession">The mongo session.</param>
        /// <returns></returns>
        public object Generate(object entity, IMongoSessionImplementor mongoSession)
        {
            return Guid.NewGuid();
        }
    }
}