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
            var memberInfo = ReflectionUtil.GetSingleMember<TRootClass>(memberName);
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
            var memberInfo = ReflectionUtil.GetSingleMember(idMember);
            return this.Id(memberInfo);
        }

        public FluentIndex<TRootClass> Index()
        {
            var fluentIndex = new FluentIndex<TRootClass>();
            this.Model.Indexes.Add(fluentIndex.Model);
            return fluentIndex;
        }

        public void UseCollection(string collectionName)
        {
            this.Model.CollectionName = collectionName;
        }
    }
}