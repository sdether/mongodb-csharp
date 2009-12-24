using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Reflection;
using System.Reflection;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentRootClassMap<T> : FluentSuperClassMap<RootClassMap, T>
    {
        private RootClassMap instance;

        public override RootClassMap Instance
        {
            get { return this.instance; }
        }

        public FluentRootClassMap()
        {
            this.instance = new RootClassMap(typeof(T));
        }

        public void Id(string memberName)
        {
            var memberInfo = this.GetSingleMember(memberName);
            this.Id(memberInfo);
        }

        public void Id(MemberInfo memberInfo)
        {
            this.instance.IdMap = new IdMap(
                memberInfo.Name,
                LateBoundReflection.GetMemberValueType(memberInfo),
                LateBoundReflection.GetGetter(memberInfo),
                LateBoundReflection.GetSetter(memberInfo));
        }

        public void Id(Expression<Func<T, string>> idMember)
        {
            var memberInfo = this.GetSingleMember(idMember);
            this.Id(memberInfo);
        }

        public void UseCollection(string collectionName)
        {
            this.instance.CollectionName = collectionName;
        }
    }
}