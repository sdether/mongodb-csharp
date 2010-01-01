using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Mapping.Models
{
    public class EmbeddedCollectionMapModel : MemberMapModel
    {
        public ICollectionType CollectionType { get; set; }

        public Type ElementType { get; set; }

        public IValueType ElementValueType { get; set; }

        public override void Accept(IMapModelVisitor visitor)
        {
            visitor.ProcessCollection(this);

            base.Accept(visitor);
        }
    }
}