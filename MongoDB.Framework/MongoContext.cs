using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Linq;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Persistence;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework
{
    public class MongoContext : IMongoContext
    {
        #region Private Fields

        private ChangeTracker changeTracker;
        private IMongoConfiguration configuration;
        private Database database;
        private Mongo mongo;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public IMongoConfiguration Configuration
        {
            get
            {
                this.EnsureNotDisposed();

                return this.configuration;
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
        /// <param name="configuration">The configuration.</param>
        /// <param name="changeTracker">The change tracker.</param>
        /// <param name="mongo">The mongo.</param>
        /// <param name="database">The database.</param>
        public MongoContext(IMongoConfiguration configuration, ChangeTracker changeTracker, Mongo mongo, Database database)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");
            if (changeTracker == null)
                throw new ArgumentNullException("changeTracker");
            if (mongo == null)
                throw new ArgumentNullException("mongo");
            if (database == null)
                throw new ArgumentNullException("database");

            this.changeTracker = changeTracker;
            this.changeTracker.Initialize(this);
            this.configuration = configuration;
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
        /// Deletes the entity on submit.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void DeleteOnSubmit(object entity)
        {
            this.EnsureNotDisposed();

            this.DeleteAllOnSubmit(new[] { entity });
        }

        /// <summary>
        /// Deletes all the entities on submit.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void DeleteAllOnSubmit(params object[] entities)
        {
            this.EnsureNotDisposed();

            this.DeleteAllOnSubmit((IEnumerable<object>)entities);
        }

        /// <summary>
        /// Deletes all the entities on submit.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void DeleteAllOnSubmit(IEnumerable<object> entities)
        {
            this.EnsureNotDisposed();

            if (entities == null)
                throw new ArgumentNullException("entities");

            foreach (var entity in entities)
                this.changeTracker.GetTrackedObject(entity).MoveToDeleted();
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
            var findOneAction = new FindOneAction(this, this.changeTracker);
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
            var findAction = new FindAction(this, this.changeTracker);
            return findAction.Find(typeof(TEntity), conditions, limit, skip, orderBy, null).Cast<TEntity>();
        }

        /// <summary>
        /// Gets the change set.
        /// </summary>
        /// <returns></returns>
        public ChangeSet GetChangeSet()
        {
            return this.changeTracker.GetChangeSet();
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
            var getByIdAction = new GetByIdAction(this, this.changeTracker);
            return getByIdAction.GetById(entityType, id);
        }

        /// <summary>
        /// Inserts the entity on submit.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void InsertOnSubmit(object entity)
        {
            this.EnsureNotDisposed();

            this.InsertAllOnSubmit(new[] { entity });
        }

        /// <summary>
        /// Inserts all the entities on submit.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void InsertAllOnSubmit(params object[] entities)
        {
            this.EnsureNotDisposed();

            this.InsertAllOnSubmit((IEnumerable<object>)entities);
        }

        /// <summary>
        /// Inserts all the entities on submit.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void InsertAllOnSubmit(IEnumerable<object> entities)
        {
            this.EnsureNotDisposed();

            if (entities == null)
                throw new ArgumentNullException("entities");

            foreach (var entity in entities)
                this.changeTracker.Track(null, entity).MoveToInserted();
        }

        /// <summary>
        /// Creates a queryable for the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> Query<TEntity>()
        {
            this.EnsureNotDisposed();

            return new MongoQueryable<TEntity>(this, this.changeTracker);
        }

        /// <summary>
        /// Submits the changes to the database.
        /// </summary>
        public void SubmitChanges()
        {
            this.EnsureNotDisposed();

            ChangeSet changeSet = this.changeTracker.GetChangeSet();
            this.PerformInserts(changeSet.Inserted);
            this.PerformUpdates(changeSet.Modified);
            this.PerformDeletes(changeSet.Deleted);
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

            this.changeTracker = null;
            this.configuration = null;
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
                var classMap = this.configuration.MappingStore.GetClassMapFor(entityGroup.Key);
                var collection = this.Database.GetCollection(classMap.CollectionName);
                foreach (var entity in entityGroup)
                {
                    new InsertAction(this, this.changeTracker)
                        .Insert(entity);
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
                var classMap = this.configuration.MappingStore.GetClassMapFor(entityGroup.Key);
                var collection = this.Database.GetCollection(classMap.CollectionName);
                foreach (var entity in entityGroup)
                {
                    new UpdateAction(this, this.changeTracker)
                        .Update(entity);
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
                var classMap = this.configuration.MappingStore.GetClassMapFor(entityGroup.Key);
                var collection = this.Database.GetCollection(classMap.CollectionName);
                foreach (var entity in entityGroup)
                {
                    new DeleteAction(this, this.changeTracker)
                        .Delete(entity);
                }
            }
        }

        #endregion
    }
}