using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework.Persistence
{
    public class InsertAction : PersistenceAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertAction"/> class.
        /// </summary>
        /// <param name="mongoContext">The mongoContext.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <param name="collection">The collection.</param>
        public InsertAction(IMongoContext mongoContext, ChangeTracker changeTracker)
            : base(mongoContext, changeTracker)
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

            var classMap = this.MongoContext.Configuration.MappingStore.GetClassMapFor(entity.GetType());
            if (!classMap.HasId)
                throw new InvalidOperationException("Only entities with identifiers are persistable.");

            var document = new Document();
            classMap.MapToDocument(entity, document);
            object id = classMap.IdMap.Generate(entity, this.MongoContext);
            document[classMap.IdMap.Key] = classMap.IdMap.ValueType.ConvertToDocumentValue(id);
            this.GetCollectionForClassMap(classMap)
                .Insert(document);
            
            var mappingContext = new MappingContext(this.MongoContext, document, entity);
            classMap.IdMap.MapFromDocument(mappingContext);
            this.ChangeTracker.GetTrackedObject(entity).MoveToPossiblyModified(mappingContext.Document);
        }
    }
}