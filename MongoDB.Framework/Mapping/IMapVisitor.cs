using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public interface IMapVisitor
    {
        void Visit(ClassMap classMap);
        void Visit(IdMap idMap);
        void Visit(MemberMap memberMap);
        void Visit(ExtendedPropertiesMap extendedPropertiesMap);
        void Visit(SimpleValueType simpleValueType);
        void Visit(NestedClassValueType nestedClassValueType);
        void Visit(CollectionValueType collectionValueType);
        void Visit(ManyToOneValueType manyToOneValueType);
        void Visit(Index index);
    }
}