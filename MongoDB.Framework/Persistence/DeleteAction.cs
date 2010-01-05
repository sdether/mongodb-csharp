using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Mapping.Visitors;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework.Persistence
{
    public class DeleteAction : PersistenceAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteAction"/> class.
        /// </summary>
        /// <param name="mongoContext">The mongoContext.</param>
        /// <param name="mongoContextCache">The mongo context cache.</param>
        /// <param name="changeTracker">The change tracker.</param>
        public DeleteAction(IMongoContextImplementor mongoContext, IMongoContextCache mongoContextCache, IChangeTracker changeTracker)
            : base(mongoContext, mongoContextCache, changeTracker)
        { }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Delete(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var classMap = this.MongoContext.MappingStore.GetClassMapFor(entity.GetType());

            var document = new DeleteDocumentMapper(this.MongoContext)
                .CreateDocument(classMap, entity);

            this.GetCollectionForClassMap(classMap).Delete(document);
            this.MongoContextCache.Remove(classMap.CollectionName, entity);
            this.ChangeTracker.StopTracking(entity);
        }
    }
}