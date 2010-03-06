using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Mapping
{
    public interface IIdGenerator
    {
        /// <summary>
        /// Generates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="mongoSession">The mongo session.</param>
        /// <returns></returns>
        object Generate(object entity, IMongoSessionImplementor mongoSession);
    }
}