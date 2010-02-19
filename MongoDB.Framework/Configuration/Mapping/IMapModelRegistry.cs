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
        /// Builds the mapping store.
        /// </summary>
        /// <returns></returns>
        IMappingStore BuildMappingStore();
    }
}