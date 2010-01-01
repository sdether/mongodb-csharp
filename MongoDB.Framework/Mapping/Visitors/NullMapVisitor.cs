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

        public virtual void ProcessSuperClass(SuperClassMap superClassMap)
        { }

        public virtual void ProcessRootClass(RootClassMap rootClassMap)
        { }

        public virtual void ProcessNestedClass(NestedClassMap nestedClassMap)
        { }

        public virtual void ProcessSubClass(SubClassMap subClassMap)
        { }

        public virtual void ProcessId(IdMap idMap)
        { }

        public virtual void ProcessMember(MemberMap memberMap)
        { }

        public virtual void ProcessExtendedProperties(ExtendedPropertiesMap extendedPropertiesMap)
        { }

        public virtual void ProcessIndex(IndexMap indexMap)
        { }

        public virtual void Visit(ClassMap classMap)
        { }

        public virtual void Visit(SuperClassMap superClassMap)
        { }

        public virtual void Visit(RootClassMap rootClassMap)
        { }

        public virtual void Visit(NestedClassMap nestedClassMap)
        { }

        public virtual void Visit(SubClassMap subClassMap)
        { }

        public virtual void Visit(IdMap idMap)
        { }

        public virtual void Visit(MemberMap memberMap)
        { }

        public virtual void Visit(ExtendedPropertiesMap extendedPropertiesMap)
        { }

        public virtual void Visit(IndexMap indexMap)
        { }
    }
}