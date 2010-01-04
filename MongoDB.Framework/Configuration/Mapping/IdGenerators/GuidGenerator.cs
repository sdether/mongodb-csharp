using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping.IdGenerators
{
    public class GuidGenerator : IIdGenerator
    {
        /// <summary>
        /// Generates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="mongoContext">The mongo context.</param>
        /// <returns></returns>
        public object Generate(object entity, IMongoContextImplementor mongoContext)
        {
            return Guid.NewGuid();
        }
    }
}