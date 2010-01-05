using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public interface IMapProvider
    {
        /// <summary>
        /// Gets the root class maps.
        /// </summary>
        /// <returns></returns>
        IEnumerable<RootClassMap> GetRootClassMaps();
    }
}