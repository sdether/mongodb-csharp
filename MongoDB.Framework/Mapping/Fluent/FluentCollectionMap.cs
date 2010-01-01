using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.Models;
using System.Collections.ObjectModel;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentCollectionMap : FluentMap<CollectionMapModel>
    {
        public FluentCollectionMap()
            : base(new CollectionMapModel())
        { }

        public FluentCollectionMap Key(string key)
        {
            this.Model.Key = key;
            return this;
        }

        public FluentCollectionMap CollectionType<TCollection>() where TCollection : ICollectionType, new()
        {
            this.Model.CollectionType = new TCollection();
            return this;
        }

        public FluentCollectionMap CollectionType(ICollectionType collectionType)
        {
            this.Model.CollectionType = collectionType;
            return this;
        }

        public FluentCollectionMap ElementType<TElement>()
        {
            this.Model.ElementType = typeof(TElement);
            return this;
        }

        public FluentCollectionMap ElementValueType<TElementValueType>() where TElementValueType : IValueType, new()
        {
            return this.ElementValueType(new TElementValueType());
        }

        public FluentCollectionMap ElementValueType(IValueType elementValueType)
        {
            this.Model.ElementValueType = elementValueType;
            return this;
        }
    }
}