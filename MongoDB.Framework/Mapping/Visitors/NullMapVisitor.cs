using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Visitors
{
    public abstract class NullMapVisitor : IMapVisitor
    {
        public virtual void Visit(ClassMapBase classMap)
        { }

        public virtual void Visit(IdMap idMap)
        { }

        public virtual void Visit(ValueTypeMemberMap memberMap)
        { }

        public virtual void Visit(ExtendedPropertiesMap extendedPropertiesMap)
        { }

        public virtual void Visit(ParentMemberMap parentMemberMap)
        { }

        public virtual void Visit(SimpleValueType simpleValueType)
        { }

        public virtual void Visit(NestedClassValueType nestedClassValueType)
        { }

        public virtual void Visit(CollectionValueType collectionValueType)
        { }

        public virtual void Visit(ManyToOneValueType manyToOneValueType)
        { }

        public virtual void Visit(Index index)
        { }
    }
}