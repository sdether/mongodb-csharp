using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping.Visitors
{
    public abstract class NullMapModelVisitor
    {
        private Dictionary<Type, Func<ModelNode, ModelNode>> funcs;

        public NullMapModelVisitor()
        {
            this.funcs = new Dictionary<Type, Func<ModelNode, ModelNode>>()
            {
                { typeof(RootClassMapModel), m => VisitRootClass((RootClassMapModel)m) },
                { typeof(NestedClassMapModel), m => VisitNestedClass((NestedClassMapModel)m) },
                { typeof(SubClassMapModel), m => VisitSubClass((SubClassMapModel)m) },
                { typeof(IdMapModel), m => VisitId((IdMapModel)m) },
                { typeof(ExtendedPropertiesMapModel), m => VisitExtendedProperties((ExtendedPropertiesMapModel)m) },
                { typeof(ConvertibleMemberMapModel), m => VisitConvertibleMember((ConvertibleMemberMapModel)m) },
                { typeof(CollectionMemberMapModel), m => VisitCollectionMember((CollectionMemberMapModel)m) },
                { typeof(PersistentMemberMapModel), m => VisitPersistentMember((PersistentMemberMapModel)m) },
                { typeof(ManyToOneMapModel), m => VisitManyToOneMember((ManyToOneMapModel)m) },
                { typeof(ParentMemberMapModel), m => VisitParentMember((ParentMemberMapModel)m) },
                { typeof(IndexModel), m => VisitIndex((IndexModel)m) }
            };
        }

        protected virtual ModelNode Visit(ModelNode modelNode)
        {
            if (modelNode == null)
                return null;

            Func<ModelNode, ModelNode> func;
            if(!funcs.TryGetValue(modelNode.GetType(), out func))
                throw new NotSupportedException();

            return func(modelNode);
        }

        protected virtual RootClassMapModel VisitRootClass(RootClassMapModel model)
        {
            return model;
        }

        protected virtual NestedClassMapModel VisitNestedClass(NestedClassMapModel model)
        {
            return model;
        }

        protected virtual SubClassMapModel VisitSubClass(SubClassMapModel model)
        {
            return model;
        }

        protected virtual IdMapModel VisitId(IdMapModel model)
        {
            return model;
        }

        protected virtual ExtendedPropertiesMapModel VisitExtendedProperties(ExtendedPropertiesMapModel model)
        {
            return model;
        }

        protected virtual ConvertibleMemberMapModel VisitConvertibleMember(ConvertibleMemberMapModel model)
        {
            return model;
        }

        protected virtual CollectionMemberMapModel VisitCollectionMember(CollectionMemberMapModel model)
        {
            return model;
        }

        protected virtual ManyToOneMapModel VisitManyToOneMember(ManyToOneMapModel model)
        {
            return model;
        }

        protected virtual PersistentMemberMapModel VisitPersistentMember(PersistentMemberMapModel model)
        {
            return model;
        }

        protected virtual ParentMemberMapModel VisitParentMember(ParentMemberMapModel model)
        {
            return model;
        }

        protected virtual IndexModel VisitIndex(IndexModel model)
        {
            return model;
        }

        protected void VisitList<T>(List<T> list) where T : ModelNode
        {
            for(int i = 0; i < list.Count; i++)
                list[i] = (T)this.Visit(list[i]);
        }
    }
}