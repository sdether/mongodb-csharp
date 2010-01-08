using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Configuration.Mapping
{
    public class CollectionMemberMapModel : MemberMapModelBase
    {
        public ICollectionType CollectionType { get; set; }

        public Type ElementType { get; set; }

        public ValueTypeBase ElementValueType { get; set; }

        public override void Accept(IMapModelVisitor visitor)
        {
            visitor.ProcessCollectionMember(this);
        }
    }
}
