using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Mapping.Models;
using MongoDB.Framework.Mapping.Types;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentRootClassMap<TRootClass> : FluentSuperClassMap<RootClassMapModel, TRootClass>
    {
        public FluentRootClassMap()
            : base(new RootClassMapModel(typeof(TRootClass)))
        { }

        public FluentIdMap Id(string memberName)
        {
            var memberInfo = this.GetSingleMember(memberName);
            return this.Id(memberInfo);
        }

        public FluentIdMap Id(MemberInfo memberInfo)
        {
            var fluentIdMap = new FluentIdMap();
            fluentIdMap.Model.Getter = memberInfo;
            fluentIdMap.Model.Setter = memberInfo;
            this.Model.IdMap = fluentIdMap.Model;
            return fluentIdMap;
        }

        public FluentIdMap Id(Expression<Func<TRootClass, object>> idMember)
        {
            var memberInfo = this.GetSingleMember(idMember);
            return this.Id(memberInfo);
        }

        public void UseCollection(string collectionName)
        {
            this.Model.CollectionName = collectionName;
        }
    }
}