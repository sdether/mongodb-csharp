using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;

namespace MongoDB.Mapper.Proxy
{
    public class CastleProxyGenerator : IProxyGenerator
    {
        private static readonly ProxyGenerator generator = new ProxyGenerator();

        /// <summary>
        /// Gets the proxy.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="id">The id.</param>
        /// <param name="mongoSession">The mongo session.</param>
        /// <returns></returns>
        public IMongoProxy GetProxy(Type entityType, object id, IMongoSessionImplementor mongoSession)
        {
            try
            {
                var initializer = new CastleLazyInitializer(entityType, id, mongoSession);

                object generatedProxy = generator.CreateClassProxy(entityType, new[] { typeof(IMongoProxy) }, initializer);
                initializer.constructed = true;
                return (IMongoProxy)generatedProxy;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to create a proxy.", ex);
            }
        }
    }
}