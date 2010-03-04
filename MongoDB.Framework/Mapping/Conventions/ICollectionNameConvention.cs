using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Conventions
{
    public interface ICollectionNameConvention
    {
        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        string GetCollectionName(Type type);
    }
}