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
    public class UpdateAction : PersistenceAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAction"/> class.
        /// </summary>
        /// <param name="mongoSession">The mongoSession.</param>
        /// <param name="mongoSessionCache">The mongo session cache.</param>
        /// <param name="changeTracker">The change tracker.</param>
        public UpdateAction(IMongoSessionImplementor mongoSession, IMongoSessionCache mongoSessionCache, IChangeTracker changeTracker)
            : base(mongoSession, mongoSessionCache, changeTracker)
        { }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Update(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var classMap = this.MongoSession.MappingStore.GetClassMapFor(entity.GetType());
            if (!classMap.HasId)
                throw new InvalidOperationException("Only entities with identifiers are persistable.");

            var document = this.MongoSession.MapToDocument(entity);
            this.GetCollectionForClassMap(classMap).Update(document);
            this.MongoSessionCache.Store(classMap.CollectionName, classMap.GetId(entity), entity);
            this.ChangeTracker.GetTrackedEntity(entity).MoveToPossibleModified(document);
        }
    }
}