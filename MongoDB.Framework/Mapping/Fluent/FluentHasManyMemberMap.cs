using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.Model;
using System.Collections.ObjectModel;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentHasManyMemberMap : FluentMemberMap<HasManyMemberMapModel>
    {

        public FluentHasManyMemberMap()
            : base(new HasManyMemberMapModel())
        { }

        public FluentHasManyMemberMap As<TCollection>() where TCollection : ICollectionType, new()
        {
            this.Model.CollectionType = new TCollection();
            return this;
        }

        public FluentHasManyMemberMap As(ICollectionType collectionType)
        {
            this.Model.CollectionType = collectionType;
            return this;
        }

        public FluentHasManyMemberMap Of<TElement>()
        {
            this.Model.ElementType = typeof(TElement);
            return this;
        }
    }
}