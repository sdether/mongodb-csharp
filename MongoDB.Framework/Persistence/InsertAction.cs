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
    public class InsertAction : PersistenceAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertAction"/> class.
        /// </summary>
        /// <param name="mongoSession">The mongoSession.</param>
        /// <param name="mongoSessionCache">The mongo session cache.</param>
        /// <param name="changeTracker">The change tracker.</param>
        public InsertAction(IMongoSessionImplementor mongoSession, IMongoSessionCache mongoSessionCache, IChangeTracker changeTracker)
            : base(mongoSession, mongoSessionCache, changeTracker)
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

            var classMap = this.MongoSession.MappingStore.GetClassMapFor(entity.GetType());
            if (!classMap.HasId)
                throw new InvalidOperationException("Only entities with identifiers are persistable.");

            var generator = new IdGenerator(this.MongoSession);
            generator.GenerateIdsFor(entity, classMap);

            var document = new EntityToDocumentMapper(this.MongoSession)
                .CreateDocument(entity);

            this.GetCollectionForClassMap(classMap)
                .Insert(document);

            var id = classMap.GetId(entity);

            this.MongoSessionCache.Store(classMap.CollectionName, id, entity);
            this.ChangeTracker.GetTrackedEntity(entity).MoveToPossibleModified(document);
        }
    }
}