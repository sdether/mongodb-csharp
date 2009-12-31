using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Models
{
    public class HasManyMemberMapModel : KeyMemberMapModel
    {
        public ICollectionType CollectionType { get; set; }

        public Type ElementType { get; set; }

        public IValueType ElementValueType { get; set; }
    }
}