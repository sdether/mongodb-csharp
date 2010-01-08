using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public class FluentCollection : FluentMember<CollectionMemberMapModel, FluentCollection>
    {
        protected override FluentCollection Fluent
        {
            get { return this; }
        }

        public FluentCollection()
            : base(new CollectionMemberMapModel())
        { }

        public FluentCollection CollectionTypeIs<TCollection>() where TCollection : ICollectionType, new()
        {
            this.Model.CollectionType = new TCollection();
            return this;
        }

        public FluentCollection CollectionTypeIs(ICollectionType collectionType)
        {
            this.Model.CollectionType = collectionType;
            return this;
        }

        public FluentCollection ElementTypeIs<TElement>()
        {
            this.Model.ElementType = typeof(TElement);
            return this;
        }
    }
}