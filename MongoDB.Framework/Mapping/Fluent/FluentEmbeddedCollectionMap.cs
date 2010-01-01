using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.Models;
using System.Collections.ObjectModel;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentEmbeddedCollectionMap : FluentBase<EmbeddedCollectionMapModel>
    {
        public FluentEmbeddedCollectionMap()
            : base(new EmbeddedCollectionMapModel())
        { }

        public FluentEmbeddedCollectionMap Key(string key)
        {
            this.Model.Key = key;
            return this;
        }

        public FluentEmbeddedCollectionMap CollectionType<TCollection>() where TCollection : ICollectionType, new()
        {
            this.Model.CollectionType = new TCollection();
            return this;
        }

        public FluentEmbeddedCollectionMap CollectionType(ICollectionType collectionType)
        {
            this.Model.CollectionType = collectionType;
            return this;
        }

        public FluentEmbeddedCollectionMap ElementType<TElement>()
        {
            this.Model.ElementType = typeof(TElement);
            return this;
        }

        public FluentEmbeddedCollectionMap ElementValueType<TElementValueType>() where TElementValueType : IValueType, new()
        {
            return this.ElementValueType(new TElementValueType());
        }

        public FluentEmbeddedCollectionMap ElementValueType(IValueType elementValueType)
        {
            this.Model.ElementValueType = elementValueType;
            return this;
        }
    }
}