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
        void Visit(ManyToOneMap manyToOneMap);
        void Visit(ExtendedPropertiesMap extendedPropertiesMap);
        void Visit(Index indexMap);
    }
}