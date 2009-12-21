using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Fluent
{
    public abstract class FluentMap<TMap> where TMap : Map
    {
        /// <summary>
        /// Gets the map instance.
        /// </summary>
        /// <value>The map instance.</value>
        public abstract TMap Instance { get; }
    }
}