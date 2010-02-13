using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping
{
    public interface IMapModelVisitor
    {
        void ProcessRootClass(RootClassMapModel rootClassMapModel);
        void ProcessNestedClass(NestedClassMapModel nestedClassMapModel);
        void ProcessSubClass(SubClassMapModel subClassMapModel);
        void ProcessSuperClass(SuperClassMapModel superClassMapModel);
        void ProcessClass(ClassMapModel classMapModel);
        void ProcessId(IdMapModel idMapModel);
        void ProcessMember(ConvertibleMemberMapModel memberMapModel);
        void ProcessCollectionMember(CollectionMemberMapModel collectionMemberMapModel);
        void ProcessManyToOne(ManyToOneMapModel manyToOneMapModel);
        void ProcessExtendedProperties(ExtendedPropertiesMapModel extendedPropertiesMapModel);
        void ProcessParentMember(ParentMemberMapModel parentMemberMapModel);
        void ProcessIndex(IndexModel indexModel);

        void Visit(RootClassMapModel rootClassMapModel);
        void Visit(NestedClassMapModel nestedClassMapModel);
        void Visit(SubClassMapModel subClassMapModel);
        void Visit(SuperClassMapModel superClassMapModel);
        void Visit(ClassMapModel classMapModel);
        void Visit(IdMapModel idMapModel);
        void Visit(ConvertibleMemberMapModel memberMapModel);
        void Visit(CollectionMemberMapModel collectionMemberMapModel);
        void Visit(ManyToOneMapModel manyToOneMapModel);
        void Visit(ExtendedPropertiesMapModel extendedPropertiesMapModel);
        void Visit(ParentMemberMapModel parentMemberMapModel);
        void Visit(IndexModel indexModel);
    }
}