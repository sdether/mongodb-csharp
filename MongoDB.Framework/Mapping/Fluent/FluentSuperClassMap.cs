﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Reflection;
using MongoDB.Framework.Mapping.Models;

namespace MongoDB.Framework.Mapping.Fluent
{
    public abstract class FluentSuperClassMap<TModel, TClass> : FluentClassMap<TModel, TClass> where TModel : SuperClassMapModel
    {
        public FluentSuperClassMap(TModel model)
            : base(model)
        { }

        public FluentDiscriminator<TDiscriminator> DiscriminateSubClassesOnKey<TDiscriminator>(string key)
        {
            var fluentDiscriminatorMap = new FluentDiscriminator<TDiscriminator>(this.Model);
            this.Model.DiscriminatorKey = key;
            return fluentDiscriminatorMap;
        }

        public FluentDiscriminator<TDiscriminator> DiscriminateSubClassesOnKey<TDiscriminator>(string key, TDiscriminator rootDiscriminatorValue)
        {
            this.Model.Discriminator = rootDiscriminatorValue;
            return this.DiscriminateSubClassesOnKey<TDiscriminator>(key);
        }

        public void ExtendedProperties(string memberName)
        {
            var memberInfo = ReflectionUtil.GetSingleMember<TClass>(memberName);
            this.ExtendedProperties(memberInfo);
        }

        public void ExtendedProperties(MemberInfo memberInfo)
        {
            this.Model.ExtendedPropertiesMap = new EmbeddedValueMapModel()
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
    }
}