using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Proxy
{
    public static class ProxyUtil
    {
        /// <summary>
        /// Guesses the type of the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Type GuessEntityType(object entity)
        {
            IMongoProxy proxy = entity as IMongoProxy;
            if (entity == null)
                return entity.GetType();

            if (proxy.LazyInitializer.IsInitialized)
                return proxy.LazyInitializer.GetImplementation().GetType();

            return proxy.LazyInitializer.EntityType;
        }
    }
}