using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;

namespace MongoDB.Framework.Configuration.Proxy.Castle
{
    public class CastleProxyGenerator : IProxyGenerator
    {
        private static readonly ProxyGenerator generator = new ProxyGenerator();

        /// <summary>
        /// Gets the proxy.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="id">The id.</param>
        /// <param name="mongoContext">The mongo context.</param>
        /// <returns></returns>
        public IMongoProxy GetProxy(Type entityType, object id, IMongoContextImplementor mongoContext)
        {
            try
            {
                var initializer = new CastleLazyInitializer(entityType, id, mongoContext);

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