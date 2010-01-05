using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Configuration.Mapping
{
    public interface IMapModelRegistry
    {
        /// <summary>
        /// Gets the root class map models.
        /// </summary>
        /// <value>The root class map models.</value>
        IEnumerable<RootClassMapModel> RootClassMapModels { get; }

        /// <summary>
        /// Builds the mapping store.
        /// </summary>
        /// <returns></returns>
        IMappingStore BuildMappingStore();
    }
}