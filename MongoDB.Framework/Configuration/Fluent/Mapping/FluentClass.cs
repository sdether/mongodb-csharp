using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Reflection;
using MongoDB.Framework.Configuration.Mapping;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public class FluentClass<TClass> : FluentClassBase<ClassMapModel, TClass>
    {
        public string CollectionName
        {
            get { return this.Model.CollectionName; }
            set { this.Model.CollectionName = value; }
        }

        public string DiscrimatorKey
        {
            get { return this.Model.DiscriminatorKey; }
            set { this.Model.DiscriminatorKey = value; }
        }

        public FluentClass()
            : base(new ClassMapModel(typeof(TClass)))
        { }

        public void ExtendedProperties(string memberName)
        {
            var memberInfo = ReflectionUtil.GetSingleMember<TClass>(memberName);
            this.ExtendedProperties(memberInfo);
        }

        public void ExtendedProperties(MemberInfo memberInfo)
        {
            this.Model.ExtendedPropertiesMap = new ExtendedPropertiesMapModel()
            {
                Getter = memberInfo,
                Setter = memberInfo
            };
        }

        public void ExtendedProperties(Expression<Func<TClass, IDictionary<string, object>>> member)
        {
            var memberInfo = ReflectionUtil.GetSingleMember(member);
            this.ExtendedProperties(memberInfo);
        }

        public FluentIndex<TClass> HasIndex()
        {
            var fluentIndex = new FluentIndex<TClass>();
            this.Model.Indexes.Add(fluentIndex.Model);
            return fluentIndex;
        }

        public FluentId Id(string memberName)
        {
            var memberInfo = ReflectionUtil.GetSingleMember<TClass>(memberName);
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

        public FluentId Id(Expression<Func<TClass, object>> idMember)
        {
            var memberInfo = ReflectionUtil.GetSingleMember(idMember);
            return this.Id(memberInfo);
        }
    }
}