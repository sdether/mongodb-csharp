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
        void ProcessMember(MemberMapModel memberMapModel);
        void ProcessExtendedProperties(ExtendedPropertiesMapModel extendedPropertiesMapModel);

        void Visit(RootClassMapModel rootClassMapModel);
        void Visit(NestedClassMapModel nestedClassMapModel);
        void Visit(SubClassMapModel subClassMapModel);
        void Visit(SuperClassMapModel superClassMapModel);
        void Visit(ClassMapModel classMapModel);
        void Visit(IdMapModel idMapModel);
        void Visit(MemberMapModel memberMapModel);
        void Visit(ExtendedPropertiesMapModel extendedPropertiesMapModel);
    }
}