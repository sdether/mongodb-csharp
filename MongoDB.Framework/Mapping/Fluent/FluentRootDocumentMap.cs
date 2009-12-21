﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentRootDocumentMap<TEntity> : FluentRootDocumentMap<RootDocumentMap, TEntity>
    {
        private RootDocumentMap instance;

        public override RootDocumentMap Instance
        {
            get { return instance; }
        }

        public FluentRootDocumentMap()
        {
            this.instance = new RootDocumentMap(typeof(TEntity));
        }
    }

    public abstract class FluentRootDocumentMap<TMap, TEntity> : FluentDocumentMap<TMap, TEntity> where TMap : RootDocumentMap
    {
        public void DiscriminatedBy<TDiscriminator>(string key, Action<FluentDiscriminatorMap<TDiscriminator>> configure)
        {
            var fluentDiscriminatorMap = new FluentDiscriminatorMap<TDiscriminator>(this.Instance);
            configure(fluentDiscriminatorMap);
            this.Instance.DiscriminatorKey = key;
        }

        public void DiscriminatedBy<TDiscriminator>(string key, TDiscriminator rootDiscriminatorValue, Action<FluentDiscriminatorMap<TDiscriminator>> configure)
        {
            this.DiscriminatedBy(key, configure);
            this.Instance.Discriminator = rootDiscriminatorValue;
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

        public void ExtendedProperties(Expression<Func<TEntity, IDictionary<string, object>>> member)
        {
            var memberInfo = this.GetSingleMember(member);
            this.ExtendedProperties(memberInfo);
        }
    }
}