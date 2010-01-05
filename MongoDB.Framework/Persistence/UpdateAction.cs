﻿using System;
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
        /// <param name="mongoContext">The mongoContext.</param>
        /// <param name="mongoContextCache">The mongo context cache.</param>
        /// <param name="changeTracker">The change tracker.</param>
        public UpdateAction(IMongoContextImplementor mongoContext, IMongoContextCache mongoContextCache, IChangeTracker changeTracker)
            : base(mongoContext, mongoContextCache, changeTracker)
        { }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Update(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var classMap = this.MongoContext.MappingStore.GetClassMapFor(entity.GetType());
            if (!classMap.HasId)
                throw new InvalidOperationException("Only entities with identifiers are persistable.");

            var mapper = new EntityToDocumentMapper(this.MongoContext);
            var document = mapper.CreateDocument(entity);
            this.GetCollectionForClassMap(classMap).Update(document);
            this.MongoContextCache.Store(classMap.CollectionName, classMap.GetId(entity), entity);
            this.ChangeTracker.GetTrackedEntity(entity).MoveToPossibleModified(document);
        }
    }
}