using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Mapping.Model;
using MongoDB.Framework.Mapping.Types;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentRootClassMap<TRootClass> : FluentSuperClassMap<RootClassMapModel, TRootClass>
    {
        public FluentRootClassMap()
            : base(new RootClassMapModel(typeof(TRootClass)))
        { }

        public void Id(string memberName)
        {
            var memberInfo = this.GetSingleMember(memberName);
            this.Id(memberInfo);
        }

        public void Id(MemberInfo memberInfo)
        {
            this.Model.IdMap = new MemberMapModel()
            {
                Getter = memberInfo,
                Setter = memberInfo
            };
        }

        public void Id(Expression<Func<TRootClass, string>> idMember)
        {
            var memberInfo = this.GetSingleMember(idMember);
            this.Id(memberInfo);
        }

        public void UseCollection(string collectionName)
        {
            this.Model.CollectionName = collectionName;
        }
    }
}