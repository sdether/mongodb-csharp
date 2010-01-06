using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Visitors
{
    public abstract class NullMapVisitor : IMapVisitor
    {
        public virtual void ProcessClass(ClassMap classMap)
        { }

        public virtual void ProcessId(IdMap idMap)
        { }

        public virtual void ProcessMember(MemberMap memberMap)
        { }

        public virtual void ProcessManyToOne(ManyToOneMap manyToOneMap)
        { }

        public virtual void ProcessExtendedProperties(ExtendedPropertiesMap extendedPropertiesMap)
        { }

        public virtual void ProcessIndex(Index indexMap)
        { }

        public virtual void Visit(ClassMap classMap)
        { }

        public virtual void Visit(IdMap idMap)
        { }

        public virtual void Visit(MemberMap memberMap)
        { }

        public virtual void Visit(ManyToOneMap manyToOneMap)
        { }

        public virtual void Visit(ExtendedPropertiesMap extendedPropertiesMap)
        { }

        public virtual void Visit(Index indexMap)
        { }
    }
}