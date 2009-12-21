using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public abstract class Map
    {
        protected MappingStore MapStore { get; private set; }

        protected Map(MappingStore mappingStore)
        {
            this.MapStore = mappingStore;
        }
    }
}