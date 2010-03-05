using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Configuration
{
    public interface IMappingStoreBuilder
    {
        IMappingStore BuildMappingStore();
    }
}