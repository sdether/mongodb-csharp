using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Linq;
using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Persistence;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework
{
    public class MongoContext : IMongoContext, IMongoContextImplementor
    {
        #region Private Fields

        private IMongoContextCache cache;
        private IChangeTracker changeTracker;
        private IMongoConfiguration configuration;
        private Database database;
        private IMappingStore mappingStore;
        private Mongo mongo;

        #endregion

        #region Explicit Properties

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        IMongoConfiguration IMongoContextImplementor.Configuration
        {
            get { return this.configuration; }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the mapping store.
        /// </summary>
        /// <value>The mapping store.</value>
        public IMappingStore MappingStore
        {
            get
            {
                this.EnsureNotDisposed();

                return this.mappingStore;
            }
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>The database.</value>
        public Database Database
        {
            get
            {
                this.EnsureNotDisposed();

                return this.database;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoContext"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="mongoContextCache">The mongo context cache.</param>
        /// <param name="mongo">The mongo.</param>
        /// <param name="database">The database.</param>
        public MongoContext(IMongoConfiguration configuration, IMappingStore mappingStore, Mongo mongo, Database database)
        {
            if (mappingStore == null)
                throw new ArgumentNullException("mappingStore");
            if (mongo == null)
                throw new ArgumentNullException("mongo");
            if (database == null)
                throw new ArgumentNullException("database");

            this.cache = new MongoContextCache();
            this.changeTracker = new ChangeTracker(this);
            this.configuration = configuration;
            this.mappingStore = mappingStore;
            this.database = database;
            this.mongo = mongo;
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
        /// Deletes the entity upon submit.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void DeleteOnSubmit(object entity)
        {
            this.EnsureNotDisposed();

            this.DeleteAllOnSubmit(new[] { entity });
        }

        /// <summary>
        /// Deletes all the entities upon submit.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void DeleteAllOnSubmit(params object[] entities)
        {
            this.EnsureNotDisposed();

            this.DeleteAllOnSubmit((IEnumerable<object>)entities);
        }

        /// <summary>
        /// Deletes all the entities upon onsubmit.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void DeleteAllOnSubmit(IEnumerable<object> entities)
        {
            this.EnsureNotDisposed();

            if (entities == null)
                throw new ArgumentNullException("entities");

            foreach (var entity in entities)
                this.changeTracker.GetTrackedEntity(entity).MoveToDeleted();
        }

        /// <summary>
        /// Finds one of the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public TEntity FindOne<TEntity>(Document conditions)
        {
            return (TEntity)this.FindOne(typeof(TEntity), conditions);
        }

        /// <summary>
        /// Finds one of the specified entity.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="conditions">The conditions.</param>
        /// <returns></returns>
        public object FindOne(Type entityType, Document conditions)
        {
            this.EnsureNotDisposed();

            var findOneAction = new FindOneAction(this, this.cache, this.changeTracker);
            return findOneAction.FindOne(entityType, conditions);
        }

        /// <summary>
        /// Finds all the entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll<TEntity>()
        {
            return this.FindAll<TEntity>(0, 0, null);
        }

        /// <summary>
        /// Finds all the entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="orderBy">The order by.</param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll<TEntity>(Document orderBy)
        {
            return this.FindAll<TEntity>(0, 0, orderBy);
        }

        /// <summary>
        /// Finds all the entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="limit">The limit.</param>
        /// <param name="skip">The skip.</param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll<TEntity>(int limit, int skip)
        {
            return this.FindAll<TEntity>(skip, limit, null);
        }

        /// <summary>
        /// Finds all the entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="limit">The limit.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll<TEntity>(int limit, int skip, Document orderBy)
        {
            return this.Find<TEntity>(null, skip, limit, orderBy);
        }

        /// <summary>
        /// Finds the specified entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> Find<TEntity>(Document conditions)
        {
            return this.Find<TEntity>(conditions, 0, 0, null);
        }

        /// <summary>
        /// Finds the specified entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="conditions"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> Find<TEntity>(Document conditions, Document orderBy)
        {
            return this.Find<TEntity>(conditions, 0, 0, orderBy);
        }

        /// <summary>
        /// Finds the specified entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="conditions">The conditions.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="skip">The skip.</param>
        /// <returns></returns>
        public IEnumerable<TEntity> Find<TEntity>(Document conditions, int limit, int skip)
        {
            return this.Find<TEntity>(conditions, skip, limit, null);
        }

        /// <summary>
        /// Finds the specified entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="conditions">The conditions.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns></returns>
        public IEnumerable<TEntity> Find<TEntity>(Document conditions, int limit, int skip, Document orderBy)
        {
            var findAction = new FindAction(this, this.cache, this.changeTracker);
            return findAction.Find(typeof(TEntity), conditions, limit, skip, orderBy, null).Cast<TEntity>();
        }

        /// <summary>
        /// Gets the entity specified by the id.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public TEntity GetById<TEntity>(object id)
        {
            return (TEntity)this.GetById(typeof(TEntity), id);
        }

        /// <summary>
        /// Gets the entity specified by the id.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public object GetById(Type entityType, object id)
        {
            this.EnsureNotDisposed();

            var getByIdAction = new GetByIdAction(this, this.cache, this.changeTracker);
            return getByIdAction.GetById(entityType, id);
        }

        /// <summary>
        /// Inserts the entity upon submit.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void InsertOnSubmit(object entity)
        {
            this.InsertAllOnSubmit(new[] { entity });
        }

        /// <summary>
        /// Inserts all the entities upon submit.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void InsertAllOnSubmit(params object[] entities)
        {
            this.InsertAllOnSubmit((IEnumerable<object>)entities);
        }

        /// <summary>
        /// Inserts all the entities upon submit.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void InsertAllOnSubmit(IEnumerable<object> entities)
        {
            this.EnsureNotDisposed();

            if (entities == null)
                throw new ArgumentNullException("entities");

            foreach (var entity in entities)
            {
                this.changeTracker.GetTrackedEntity(entity).MoveToInserted();
            }
        }

        /// <summary>
        /// Creates a queryable for the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> Query<TEntity>()
        {
            this.EnsureNotDisposed();

            return new MongoQueryable<TEntity>(this, this.cache, this.changeTracker);
        }

        /// <summary>
        /// Submits the changes.
        /// </summary>
        public void SubmitChanges()
        {
            var changeSet = this.changeTracker.GetChangeSet();
            foreach (var inserted in changeSet.Inserted)
                this.PerformInsert(inserted);
            foreach (var modified in changeSet.Modified)
                this.PerformUpdate(modified);
            foreach (var deleted in changeSet.Deleted)
                this.PerformDelete(deleted);
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

            this.cache.Clear();
            this.cache = null;
            this.changeTracker.Dispose();
            this.changeTracker = null;
            this.mappingStore = null;
            this.database = null;
            this.mongo.Disconnect();
            this.mongo = null;
        }

        /// <summary>
        /// Ensures the not disposed.
        /// </summary>
        protected void EnsureNotDisposed()
        {
            if (this.mongo == null)
                throw new ObjectDisposedException("MongoContext");
        }

        protected virtual void PerformInsert(object entity)
        {
            var action = new InsertAction(this, this.cache, this.changeTracker);
            action.Insert(entity);
        }

        protected virtual void PerformUpdate(object entity)
        {
            var action = new UpdateAction(this, this.cache, this.changeTracker);
            action.Update(entity);
        }

        /// <summary>
        /// Performs the delete.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected virtual void PerformDelete(object entity)
        {
            var action = new DeleteAction(this, this.cache, this.changeTracker);
            action.Delete(entity);
        }

        #endregion
    }
}