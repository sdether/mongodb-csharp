using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Linq;

namespace MongoDB.Framework
{
    public class MongoContext : IDisposable
    {
        #region Private Fields

        private Mongo mongo;
        private EntityMapper entityMapper;

        private List<object> entitiesToInsert;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public MongoConfiguration Configuration { get; private set; }

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
        public MongoContext(MongoConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            this.mongo = new Mongo();
            this.mongo.Connect();

            this.Configuration = configuration;
            this.Database = this.mongo.getDB(configuration.DatabaseName);

            this.entityMapper = new EntityMapper(configuration);
            this.entitiesToInsert = new List<object>();

            this.Initialize();
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
        /// Initializes the mongo database with indexes, etc...
        /// </summary>
        public virtual void Initialize()
        {
            this.EnsureIndexes();
        }

        /// <summary>
        /// Inserts the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Insert<TEntity>(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            this.entitiesToInsert.Add(entity);
        }

        /// <summary>
        /// Inserts all the entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities.</param>
        public void InsertAll<TEntity>(params TEntity[] entities)
        {
            this.InsertAll((IEnumerable<TEntity>)entities);
        }

        /// <summary>
        /// Inserts all the entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities.</param>
        public void InsertAll<TEntity>(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            this.entitiesToInsert.AddRange(entities.Cast<object>());
        }

        /// <summary>
        /// Queries this instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> Query<TEntity>()
        {
            return new MongoQueryable<TEntity>(this.Database, this.entityMapper);
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
            Dictionary<string, List<Document>> documentCollections = new Dictionary<string, List<Document>>();
            foreach(var entityGroup in this.entitiesToInsert.GroupBy(e => e.GetType()))
            {
                var rootEntityMap = this.Configuration.GetRootEntityMapFor(entityGroup.Key);
                List<Document> documents;
                if (!documentCollections.TryGetValue(rootEntityMap.CollectionName, out documents))
                    documentCollections[rootEntityMap.CollectionName] = documents = new List<Document>();
                foreach (var entity in entityGroup)
                    documents.Add(this.entityMapper.MapEntityToDocument(entity));
            }

            foreach (var documentCollection in documentCollections)
            {
                this.Database.GetCollection(documentCollection.Key)
                    .Insert(documentCollection.Value);
            }

            this.entitiesToInsert.Clear();
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

        private void EnsureIndexes()
        {
            foreach (var rootEntityMap in this.Configuration.RootEntityMaps)
            {
                IMongoCollection collection = this.Database.GetCollection(rootEntityMap.CollectionName);

                foreach (var index in rootEntityMap.Indexes)
                {
                    Document fieldsAndDirections = new Document();
                    foreach (var pair in index.DocumentKeys)
                        fieldsAndDirections.Add(pair.Key, pair.Value == IndexDirection.Ascending ? 1 : -1);

                    collection.MetaData.CreateIndex(fieldsAndDirections, index.IsUnique);
                }
            }
        }

        #endregion
    }
}