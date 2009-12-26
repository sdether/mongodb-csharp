using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework.Persistence
{
    public class InsertAction : PersistenceAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertAction"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <param name="collection">The collection.</param>
        public InsertAction(MappingStore mappingStore, ChangeTracker changeTracker, IMongoCollection collection)
            : base(mappingStore, changeTracker, collection)
        { }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Insert(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var classMap = this.MappingStore.GetClassMapFor(entity.GetType());
            if (!classMap.HasId)
                throw new InvalidOperationException("Only entities with identifiers are persistable.");


            var mappingContext = new MappingContext(this.MappingStore, entity);
            classMap.Map(mappingContext);
            this.Collection.Insert(mappingContext.Document);
            mappingContext = new MappingContext(this.MappingStore, mappingContext.Document, classMap.Type);
            classMap.IdMap.TranslateFromDocument(mappingContext);
            this.ChangeTracker.GetTrackedObject(entity).MoveToPossibleModified(mappingContext.Document);
        }
    }
}