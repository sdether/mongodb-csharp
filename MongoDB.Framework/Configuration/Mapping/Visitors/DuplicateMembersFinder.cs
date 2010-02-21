using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Visitors
{
    public class DuplicateMembersFinder : DefaultMapModelVisitor
    {
        public void FindAndClean(ClassMapModel classMapModel)
        {
            this.Visit(classMapModel);
        }

        protected override RootClassMapModel VisitRootClass(RootClassMapModel model)
        {
            this.ProcessSuperClass(model);
            return base.VisitRootClass(model);
        }

        protected override NestedClassMapModel VisitNestedClass(NestedClassMapModel model)
        {
            this.ProcessSuperClass(model);
            return base.VisitNestedClass(model);
        }

        private void ProcessSuperClass(SuperClassMapModel model)
        {
            if (model.IdMap != null)
                model.PersistentMemberMaps.RemoveAll(m => AreMembersEqual(m.Getter, model.IdMap.Getter));

            if (model.ExtendedPropertiesMap != null)
                model.PersistentMemberMaps.RemoveAll(m => AreMembersEqual(m.Getter, model.ExtendedPropertiesMap.Getter));

            foreach (var subModel in model.SubClassMaps)
                this.RemoveDuplicateMemberMapsFromSubClass(model, subModel);
        }

        private void RemoveDuplicateMemberMapsFromSubClass(SuperClassMapModel model, SubClassMapModel subModel)
        {
            subModel.PersistentMemberMaps.RemoveAll(m => model.PersistentMemberMaps.Any(x => AreMembersEqual(m.Getter, x.Getter)));

            if(model.IdMap != null)
                subModel.PersistentMemberMaps.RemoveAll(m => AreMembersEqual(m.Getter, model.IdMap.Getter));

            if(model.ExtendedPropertiesMap != null)
                subModel.PersistentMemberMaps.RemoveAll(m => AreMembersEqual(m.Getter, model.ExtendedPropertiesMap.Getter));
        }

        private bool AreMembersEqual(MemberInfo member1, MemberInfo member2)
        {
            return member1.DeclaringType == member2.DeclaringType
                && member1.Name == member2.Name;
        }
    }
}