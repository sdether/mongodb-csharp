using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.Models;
using System.Collections.ObjectModel;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentHasManyMemberMap : FluentMemberMap<HasManyMemberMapModel>
    {

        public FluentHasManyMemberMap()
            : base(new HasManyMemberMapModel())
        { }

        public FluentHasManyMemberMap CollectionType<TCollection>() where TCollection : ICollectionType, new()
        {
            this.Model.CollectionType = new TCollection();
            return this;
        }

        public FluentHasManyMemberMap CollectionType(ICollectionType collectionType)
        {
            this.Model.CollectionType = collectionType;
            return this;
        }

        public FluentHasManyMemberMap ElementType<TElement>()
        {
            this.Model.ElementType = typeof(TElement);
            return this;
        }

        public FluentHasManyMemberMap ElementValueType<TElementValueType>() where TElementValueType : IValueType, new()
        {
            return this.ElementValueType(new TElementValueType());
        }

        public FluentHasManyMemberMap ElementValueType(IValueType elementValueType)
        {
            this.Model.ElementValueType = elementValueType;
            return this;
        }
    }
}