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
            this.Model.CollectionPart = new EmbeddedCollectionPart();
            var fluentCollectionPart = new FluentEmbeddedCollectionPart(this.Model.CollectionPart);
            configure(fluentCollectionPart);
            return this;
        }

        public FluentEmbeddedMemberMap<TEntity> AsValue(Action<FluentEmbeddedValuePart> configure)
        {
            this.Model.ValuePart = new EmbeddedValuePart();
            var fluentValuePart = new FluentEmbeddedValuePart(this.Model.ValuePart);
            configure(fluentValuePart);
            return this;
        }
    }
}