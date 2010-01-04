using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Configuration.Mapping.Models;
using MongoDB.Framework.Configuration.Mapping.Types;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Fluent
{
    public class FluentRootClass<TRootClass> : FluentSuperClass<RootClassMapModel, TRootClass>
    {
        public FluentRootClass()
            : base(new RootClassMapModel(typeof(TRootClass)))
        { }

        public FluentId Id(string memberName)
        {
            var memberInfo = ReflectionUtil.GetSingleMember<TRootClass>(memberName);
            return this.Id(memberInfo);
        }

        public FluentId Id(MemberInfo memberInfo)
        {
            var fluentIdMap = new FluentId();
            fluentIdMap.Model.Getter = memberInfo;
            fluentIdMap.Model.Setter = memberInfo;
            this.Model.IdMap = fluentIdMap.Model;
            return fluentIdMap;
        }

        public FluentId Id(Expression<Func<TRootClass, object>> idMember)
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