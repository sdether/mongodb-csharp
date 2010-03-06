using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Mapper.Mapping;

namespace MongoDB.Mapper.Configuration
{
    public interface IMappingStoreBuilder
    {
        IMappingStore BuildMappingStore();
    }
}