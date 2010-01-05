using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Proxy
{
    /// <remarks>
    /// This was wholesale ripped out of NHibernate.
    /// </remarks>
    public interface IProxyGenerator
    {
        /// <summary>
        /// Gets the proxy.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="id">The id.</param>
        /// <param name="mongoContext">The mongo context.</param>
        /// <returns></returns>
        IMongoProxy GetProxy(Type entityType, object id, IMongoContextImplementor mongoContext);
    }
}