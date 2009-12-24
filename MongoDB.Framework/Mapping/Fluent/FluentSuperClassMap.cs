using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Mapping.Fluent
{
    public abstract class FluentSuperClassMap<TMap, T> : FluentClassMap<TMap, T> where TMap : SuperClassMap 
    {
        public FluentDiscriminatorMap<TDiscriminator> DiscriminateSubClassesOnKey<TDiscriminator>(string key)
        {
            var fluentDiscriminatorMap = new FluentDiscriminatorMap<TDiscriminator>(this.Instance);
            this.Instance.DiscriminatorKey = key;
            return fluentDiscriminatorMap;
        }

        public FluentDiscriminatorMap<TDiscriminator> DiscriminateSubClassesOnKey<TDiscriminator>(string key, TDiscriminator rootDiscriminatorValue)
        {
            this.Instance.Discriminator = rootDiscriminatorValue;
            return this.DiscriminateSubClassesOnKey<TDiscriminator>(key);
        }

        public void ExtendedProperties(string memberName)
        {
            var memberInfo = this.GetSingleMember(memberName);
            this.ExtendedProperties(memberInfo);
        }

        public void ExtendedProperties(MemberInfo memberInfo)
        {
            this.Instance.ExtendedPropertiesMap = new ExtendedPropertiesMap(
                memberInfo.Name,
                LateBoundReflection.GetMemberValueType(memberInfo),
                LateBoundReflection.GetGetter(memberInfo),
                LateBoundReflection.GetSetter(memberInfo));
        }

        public void ExtendedProperties(Expression<Func<T, IDictionary<string, object>>> member)
        {
            var memberInfo = this.GetSingleMember(member);
            this.ExtendedProperties(memberInfo);
        }
    }
}