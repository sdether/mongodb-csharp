using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.Models;
using System.Collections.ObjectModel;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentCollectionMemberMap : FluentMemberMap<CollectionMemberMapModel>
    {

        public FluentCollectionMemberMap()
            : base(new CollectionMemberMapModel())
        { }

        public FluentCollectionMemberMap Key(string key)
        {
            this.Model.Key = key;
            return this;
        }

        public FluentCollectionMemberMap CollectionType<TCollection>() where TCollection : ICollectionType, new()
        {
            this.Model.CollectionType = new TCollection();
            return this;
        }

        public FluentCollectionMemberMap CollectionType(ICollectionType collectionType)
        {
            this.Model.CollectionType = collectionType;
            return this;
        }

        public FluentCollectionMemberMap ElementType<TElement>()
        {
            this.Model.ElementType = typeof(TElement);
            return this;
        }

        public FluentCollectionMemberMap ElementValueType<TElementValueType>() where TElementValueType : IValueType, new()
        {
            return this.ElementValueType(new TElementValueType());
        }

        public FluentCollectionMemberMap ElementValueType(IValueType elementValueType)
        {
            this.Model.ElementValueType = elementValueType;
            return this;
        }
    }
}