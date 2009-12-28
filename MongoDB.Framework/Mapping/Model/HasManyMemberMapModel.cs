using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Model
{
    public class HasManyMemberMapModel : KeyMemberMapModel
    {
        public ICollectionType CollectionType { get; set; }

        public Type ElementType { get; set; }

        public IValueType ElementValueType { get; set; }
    }
}