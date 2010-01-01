using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.Models;
using MongoDB.Framework.Reflection;
using System.Reflection;
using System.Linq.Expressions;
using MongoDB.Framework.Linq.Visitors;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentEmbeddedMemberMap<TEntity> : FluentBase<MemberMapModel>
    {
        public FluentEmbeddedMemberMap()
            : base(new MemberMapModel())
        { }

        public FluentEmbeddedMemberMap<TEntity> Key(string key)
        {
            this.Model.Key = key;
            return this;
        }

        public FluentEmbeddedMemberMap<TEntity> AsCollection(Action<FluentEmbeddedCollectionPart> configure)
        {
            var collectionPart = new EmbeddedCollectionPart();
            var fluentCollectionPart = new FluentEmbeddedCollectionPart(collectionPart);
            configure(fluentCollectionPart);
            this.Model.Part = collectionPart;
            return this;
        }

        public FluentEmbeddedMemberMap<TEntity> AsNestedClass<TNestedClass>(Action<FluentNestedClassMap<TNestedClass>> configure)
        {
            var classPart = new EmbeddedClassPart();
            var fluentNestedClassMap = new FluentNestedClassMap<TNestedClass>();
            classPart.NestedClassMap = fluentNestedClassMap.Model;
            configure(fluentNestedClassMap);
            this.Model.Part = classPart;
            return this;
        }

        public FluentEmbeddedMemberMap<TEntity> AsValue(Action<FluentEmbeddedValuePart> configure)
        {
            var valuePart = new EmbeddedValuePart();
            var fluentValuePart = new FluentEmbeddedValuePart(valuePart);
            configure(fluentValuePart);
            this.Model.Part = valuePart;
            return this;
        }
    }
}