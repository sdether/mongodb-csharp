﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Linq;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework
{
    public class MongoContext : IDisposable
    {
        #region Private Fields

        private ChangeTracker changeTracker;
        private Mongo mongo;
        private EntityMapper entityMapper;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>The database.</value>
        public Database Database { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoContext&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public MongoContext(EntityMapper entityMapper, ChangeTracker changeTracker, Mongo mongo, Database database)
        {
            if (entityMapper == null)
                throw new ArgumentNullException("entityMapper");
            if (changeTracker == null)
                throw new ArgumentNullException("changeTracker");
            if (mongo == null)
                throw new ArgumentNullException("mongo");
            if (database == null)
                throw new ArgumentNullException("database");

            this.mongo = mongo;
            this.Database = database;

            this.entityMapper = entityMapper;
            this.changeTracker = changeTracker;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="MongoContext"/> is reclaimed by garbage collection.
        /// </summary>
        ~MongoContext()
        {
            this.Dispose(false);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Inserts the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void InsertOnSubmit<TEntity>(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            this.changeTracker.Track(null, entity).MoveToAdded();
        }

        /// <summary>
        /// Inserts all the entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities.</param>
        public void InsertAllOnSubmit<TEntity>(params TEntity[] entities)
        {
            this.InsertAllOnSubmit((IEnumerable<TEntity>)entities);
        }

        /// <summary>
        /// Inserts all the entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities.</param>
        public void InsertAllOnSubmit<TEntity>(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            foreach (var entity in entities)
                this.changeTracker.Track(null, entity).MoveToAdded();
        }

        /// <summary>
        /// Queries this instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> Query<TEntity>()
        {
            return new MongoQueryable<TEntity>(this.Database, this.entityMapper, this.changeTracker);
        }

        /// <summary>
        /// Sends the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public Document SendCommand(string command)
        {
            return this.Database.SendCommand(command);
        }

        /// <summary>
        /// Sends the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public Document SendCommand(Document command)
        {
            return this.Database.SendCommand(command);
        }

        /// <summary>
        /// Submits the changes.
        /// </summary>
        public void SubmitChanges()
        {
            ChangeSet changeSet = this.changeTracker.GetChangeSet();
            this.PerformInserts(changeSet.Added);
            this.PerformUpdates(changeSet.Modified);
            this.PerformDeletes(changeSet.Removed);
        }

        #endregion

        #region Protected Members

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            this.mongo.Disconnect();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Performs the adds.
        /// </summary>
        /// <param name="added">The added.</param>
        private void PerformInserts(IList<object> inserted)
        {
            foreach (var entityGroup in inserted.GroupBy(a => a.GetType()))
            {
                var rootEntityMap = this.entityMapper.Configuration.GetRootEntityMapFor(entityGroup.Key);
                var collection = this.Database.GetCollection(rootEntityMap.CollectionName);
                foreach (var entity in entityGroup)
                {
                    var document = this.entityMapper.MapEntityToDocument(entity);
                    collection.Insert(document);
                    rootEntityMap.IdMap.SetDocumentValueOnEntity(entity, document["_id"]);
                    this.changeTracker.GetTrackedObject(entity).MoveToPossibleModified(document);
                }
            }
        }

        /// <summary>
        /// Performs the updates.
        /// </summary>
        /// <param name="updated">The updated.</param>
        private void PerformUpdates(IList<object> updated)
        {
            foreach (var entityGroup in updated.GroupBy(a => a.GetType()))
            {
                var rootEntityMap = this.entityMapper.Configuration.GetRootEntityMapFor(entityGroup.Key);
                var collection = this.Database.GetCollection(rootEntityMap.CollectionName);
                foreach (var entity in entityGroup)
                {
                    var document = this.entityMapper.MapEntityToDocument(entity);
                    collection.Update(document);
                    this.changeTracker.GetTrackedObject(entity).MoveToPossibleModified(document);
                }
            }
        }

        /// <summary>
        /// Performs the removes.
        /// </summary>
        /// <param name="removed">The removed.</param>
        private void PerformDeletes(IList<object> deleted)
        {
            foreach (var entityGroup in deleted.GroupBy(a => a.GetType()))
            {
                var rootEntityMap = this.entityMapper.Configuration.GetRootEntityMapFor(entityGroup.Key);
                var collection = this.Database.GetCollection(rootEntityMap.CollectionName);
                foreach (var entity in entityGroup)
                {
                    //TODO: investigate using the in clause for deleting multiples...
                    var document = new Document().Append("_id", new Oid((string)rootEntityMap.IdMap.Getter(entity)));
                    collection.Delete(document);
                    this.changeTracker.GetTrackedObject(entity).MoveToDead();
                }
            }
        }

        #endregion
    }
}