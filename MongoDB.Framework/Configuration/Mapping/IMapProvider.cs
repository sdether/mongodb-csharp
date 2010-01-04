using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping
{
    public interface IMapProvider
    {
        /// <summary>
        /// Gets the root class map for.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The RootClassMap if it exists; otherwise <c>null</c>.</returns>
        RootClassMap GetRootClassMapFor(Type type);
    }
}