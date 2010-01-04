using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping
{
    public interface IMapVisitor
    {
        void ProcessRootClass(RootClassMap rootClassMap);
        void ProcessNestedClass(NestedClassMap nestedClassMap);
        void ProcessSubClass(SubClassMap subClassMap);
        void ProcessSuperClass(SuperClassMap superClassMap);
        void ProcessClass(ClassMap classMap);
        void ProcessId(IdMap idMap);
        void ProcessMember(MemberMap memberMap);
        void ProcessManyToOne(ManyToOneMap manyToOneMap);
        void ProcessExtendedProperties(ExtendedPropertiesMap extendedPropertiesMap);
        void ProcessIndex(Index indexMap);

        void Visit(RootClassMap rootClassMap);
        void Visit(NestedClassMap nestedClassMap);
        void Visit(SubClassMap subClassMap);
        void Visit(SuperClassMap superClassMap);
        void Visit(ClassMap classMap);
        void Visit(IdMap idMap);
        void Visit(MemberMap memberMap);
        void Visit(ManyToOneMap manyToOneMap);
        void Visit(ExtendedPropertiesMap extendedPropertiesMap);
        void Visit(Index indexMap);
    }
}