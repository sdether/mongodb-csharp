using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Tracking;
using MongoDB.Framework.Mapping.Visitors;

namespace MongoDB.Framework.Persistence
{
    public class DeleteAction : PersistenceAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteAction"/> class.
        /// </summary>
        /// <param name="mongoContext">The mongoContext.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <param name="collection">The collection.</param>
        public DeleteAction(IMongoContext mongoContext, ChangeTracker changeTracker)
            : base(mongoContext, changeTracker)
        { }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Delete(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var classMap = this.MongoContext.Configuration.MappingStore.GetClassMapFor(entity.GetType());

            var document = new DeleteDocumentMapper(this.MongoContext)
                .CreateDocument(classMap, entity);

            this.GetCollectionForClassMap(classMap).Delete(document);
            this.ChangeTracker.GetTrackedObject(entity).MoveToDead();
        }
    }
}