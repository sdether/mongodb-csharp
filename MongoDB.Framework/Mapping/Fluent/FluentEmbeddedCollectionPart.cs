using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.Models;
using System.Collections.ObjectModel;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentEmbeddedCollectionPart
    {
        private EmbeddedCollectionPart embeddedCollectionMapModel;

        public FluentEmbeddedCollectionPart(EmbeddedCollectionPart embeddedCollectionMapModel)
        {
            this.embeddedCollectionMapModel = embeddedCollectionMapModel;
        }

        public void CollectionTypeIs<TCollection>() where TCollection : ICollectionType, new()
        {
            this.embeddedCollectionMapModel.CollectionType = new TCollection();
        }

        public void CollectionTypeIs(ICollectionType collectionType)
        {
            this.embeddedCollectionMapModel.CollectionType = collectionType;
        }

        public void ElementTypeIs<TElement>()
        {
            this.embeddedCollectionMapModel.ElementType = typeof(TElement);
        }

        public void ElementValueTypeIs<TElementValueType>() where TElementValueType : IValueType, new()
        {
            this.ElementValueTypeIs(new TElementValueType());
        }

        public void ElementValueTypeIs(IValueType elementValueType)
        {
            this.embeddedCollectionMapModel.ElementValueType = elementValueType;
        }
    }
}