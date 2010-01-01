using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Models.Visitors
{
    public abstract class NullMapModelVisitor : IMapModelVisitor
    {
        public virtual void ProcessRootClass(RootClassMapModel rootClassMapModel)
        { }

        public virtual void ProcessNestedClass(NestedClassMapModel nestedClassMapModel)
        { }

        public virtual void ProcessSubClass(SubClassMapModel subClassMapModel)
        { }

        public virtual void ProcessSuperClass(SuperClassMapModel superClassMapModel)
        { }

        public virtual void ProcessClass(ClassMapModel classMapModel)
        { }

        public virtual void ProcessId(IdMapModel idMapModel)
        { }

        public virtual void ProcessCollection(EmbeddedCollectionMapModel collectionMapModel)
        { }

        public virtual void ProcessValue(EmbeddedValueMapModel valueMapModel)
        { }

        public virtual void ProcessMember(MemberMapModel memberMapModel)
        { }

        public virtual void ProcessExtendedProperties(ExtendedPropertiesMapModel extendedPropertiesMapModel)
        { }

        public virtual void Visit(RootClassMapModel rootClassMapModel)
        { }

        public virtual void Visit(NestedClassMapModel nestedClassMapModel)
        { }

        public virtual void Visit(SubClassMapModel subClassMapModel)
        { }

        public virtual void Visit(SuperClassMapModel superClassMapModel)
        { }

        public virtual void Visit(ClassMapModel classMapModel)
        { }

        public virtual void Visit(IdMapModel idMapModel)
        { }

        public virtual void Visit(EmbeddedCollectionMapModel collectionMapModel)
        { }

        public virtual void Visit(EmbeddedValueMapModel valueMapModel)
        { }

        public virtual void Visit(MemberMapModel memberMapModel)
        { }

        public virtual void Visit(ExtendedPropertiesMapModel extendedPropertiesMapModel)
        { }
    }
}
