using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping.Visitors
{
    public abstract class DefaultMapModelVisitor : MapModelVisitor
    {
        protected override RootClassMapModel VisitRootClass(RootClassMapModel model)
        {
            this.VisitList(model.Indexes);
            if(model.IdMap != null)
                model.IdMap = (IdMapModel)this.Visit(model.IdMap);
            if(model.ExtendedPropertiesMap != null)
                model.ExtendedPropertiesMap = (ExtendedPropertiesMapModel)this.Visit(model.ExtendedPropertiesMap);
            this.VisitList(model.PersistentMemberMaps);
            this.VisitList(model.SubClassMaps);
            return model;
        }

        protected override NestedClassMapModel VisitNestedClass(NestedClassMapModel model)
        {
            if(model.ParentMap != null)
                model.ParentMap = (ParentMemberMapModel)this.Visit(model.ParentMap);
            if (model.IdMap != null)
                model.IdMap = (IdMapModel)this.Visit(model.IdMap);
            if (model.ExtendedPropertiesMap != null)
                model.ExtendedPropertiesMap = (ExtendedPropertiesMapModel)this.Visit(model.ExtendedPropertiesMap);
            this.VisitList(model.PersistentMemberMaps);
            this.VisitList(model.SubClassMaps);
            return model;
        }

        protected override SubClassMapModel VisitSubClass(SubClassMapModel model)
        {
            this.VisitList(model.PersistentMemberMaps);
            return model;
        }
    }
}