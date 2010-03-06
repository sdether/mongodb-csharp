using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Mapper.Mapping;

namespace MongoDB.Mapper.Configuration.Mapping
{
    public class CollectionMemberMapModel : PersistentMemberMapModel
    {
        public ICollectionType CollectionType { get; set; }

        public Type ElementType { get; set; }

        public ValueTypeBase ElementValueType { get; set; }
    }
}
