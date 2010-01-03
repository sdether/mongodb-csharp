using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Models.Visitors
{
    public abstract class DefaultMapModelVisitor : NullMapModelVisitor
    {
        public override void Visit(ClassMapModel classMapModel)
        {
            classMapModel.Accept(this);
        }

        public override void Visit(ExtendedPropertiesMapModel extendedPropertiesMapModel)
        {
            extendedPropertiesMapModel.Accept(this);
        }

        public override void Visit(IdMapModel idMapModel)
        {
            idMapModel.Accept(this);
        }

        public override void Visit(MemberMapModel memberMapModel)
        {
            memberMapModel.Accept(this);
        }

        public override void Visit(ManyToOneMapModel manyToOneMapModel)
        {
            manyToOneMapModel.Accept(this);
        }

        public override void Visit(NestedClassMapModel nestedClassMapModel)
        {
            nestedClassMapModel.Accept(this);
        }

        public override void Visit(RootClassMapModel rootClassMapModel)
        {
            rootClassMapModel.Accept(this);
        }

        public override void Visit(SubClassMapModel subClassMapModel)
        {
            subClassMapModel.Accept(this);
        }

        public override void Visit(SuperClassMapModel superClassMapModel)
        {
            superClassMapModel.Accept(this);
        }

        public override void Visit(IndexModel indexModel)
        {
            indexModel.Accept(this);
        }
    }
}