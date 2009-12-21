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
    public class FluentCollectionMap<TEntity> : FluentRootDocumentMap<CollectionMap, TEntity>
    {
        private CollectionMap instance;

        public override CollectionMap Instance
        {
            get { return this.instance; }
        }

        public FluentCollectionMap()
        {
            this.instance = new CollectionMap(typeof(TEntity));
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

        public void Id(Expression<Func<TEntity, string>> idMember)
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