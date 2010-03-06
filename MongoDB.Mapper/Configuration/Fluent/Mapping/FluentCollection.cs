using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Mapper.Configuration.Mapping;
using MongoDB.Mapper.Mapping;

namespace MongoDB.Mapper.Configuration.Fluent.Mapping
{
    public class FluentCollection : FluentPersistentMember<CollectionMemberMapModel, FluentCollection>
    {
        protected override FluentCollection Fluent
        {
            get { return this; }
        }

        public FluentCollection()
            : base(new CollectionMemberMapModel())
        { }

        public FluentCollection TypeIs<TCollection>() where TCollection : ICollectionType, new()
        {
            this.Model.CollectionType = new TCollection();
            return this;
        }

        public FluentCollection TypeIs(ICollectionType collectionType)
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