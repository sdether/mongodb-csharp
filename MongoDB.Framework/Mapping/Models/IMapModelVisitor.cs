using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Models
{
    public interface IMapModelVisitor
    {
        void ProcessRootClass(RootClassMapModel rootClassMapModel);
        void ProcessNestedClass(NestedClassMapModel nestedClassMapModel);
        void ProcessSubClass(SubClassMapModel subClassMapModel);
        void ProcessSuperClass(SuperClassMapModel superClassMapModel);
        void ProcessClass(ClassMapModel classMapModel);
        void ProcessId(IdMapModel idMapModel);
        void ProcessCollection(EmbeddedCollectionMapModel collectionMapModel);
        void ProcessValue(EmbeddedValueMapModel valueMapModel);
        void ProcessMember(MemberMapModel memberMapModel);
        void ProcessExtendedProperties(ExtendedPropertiesMapModel extendedPropertiesMapModel);

        void Visit(RootClassMapModel rootClassMapModel);
        void Visit(NestedClassMapModel nestedClassMapModel);
        void Visit(SubClassMapModel subClassMapModel);
        void Visit(SuperClassMapModel superClassMapModel);
        void Visit(ClassMapModel classMapModel);
        void Visit(IdMapModel idMapModel);
        void Visit(EmbeddedCollectionMapModel collectionMapModel);
        void Visit(EmbeddedValueMapModel valueMapModel);
        void Visit(MemberMapModel memberMapModel);
        void Visit(ExtendedPropertiesMapModel extendedPropertiesMapModel);
    }
}