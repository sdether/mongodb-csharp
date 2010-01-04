using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping.Models
{
    public class CollectionMapModel : MemberMapModelBase
    {
        public ICollectionType CollectionType { get; set; }

        public Type ElementType { get; set; }

        public IValueType ElementValueType { get; set; }

        public override void Accept(IMapModelVisitor visitor)
        {
            visitor.ProcessCollection(this);
        }
    }
}
