using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Configuration.Mapping.Models;
using System.Collections.ObjectModel;

namespace MongoDB.Framework.Configuration.Mapping.Fluent
{
    public class FluentCollection : FluentMember<CollectionMapModel, FluentCollection>
    {
        protected override FluentCollection Fluent
        {
            get { return this; }
        }

        public FluentCollection()
            : base(new CollectionMapModel())
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

        public FluentCollection ElementValueTypeIs<TElementValueType>() where TElementValueType : IValueType, new()
        {
            return this.ElementValueTypeIs(new TElementValueType());
        }

        public FluentCollection ElementValueTypeIs(IValueType elementValueType)
        {
            this.Model.ElementValueType = elementValueType;
            return this;
        }
    }
}