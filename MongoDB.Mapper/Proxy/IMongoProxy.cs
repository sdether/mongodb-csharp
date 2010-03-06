using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Proxy
{
    public interface IMongoProxy
    {
        /// <summary>
        /// Gets the lazy initializer.
        /// </summary>
        /// <value>The lazy initializer.</value>
        ILazyInitializer LazyInitializer { get; }
    }
}
