using System;
using System.Collections.Generic;
using System.Linq;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Tracking;
using MongoDB.Framework.Configuration.Mapping;

namespace MongoDB.Framework
{
    public interface IMongoContext : IDisposable
    {
        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>The database.</value>
        Database Database { get; }

        /// <summary>
        /// Gets the mapping store.
        /// </summary>
        /// <value>The mapping store.</value>
        IMappingStore MappingStore { get; }

        /// <summary>
        /// Deletes all the entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void DeleteAll(params object[] entities);

        /// <summary>
        /// Deletes all the entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void DeleteAll(IEnumerable<object> entities);

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(object entity);

        /// <summary>
        /// Gets the entity specified by the id.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        TEntity GetById<TEntity>(object id);

        /// <summary>
        /// Gets the by id.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        object GetById(Type entityType, object id);

        /// <summary>
        /// Finds one of the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        TEntity FindOne<TEntity>(Document conditions);

        /// <summary>
        /// Finds one of the specified entity.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="conditions">The conditions.</param>
        /// <returns></returns>
        object FindOne(Type entityType, Document conditions);

        /// <summary>
        /// Finds all the entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll<TEntity>();

        /// <summary>
        /// Finds all the entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="orderBy">The order by.</param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll<TEntity>(Document orderBy);

        /// <summary>
        /// Finds all the entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="limit">The limit.</param>
        /// <param name="skip">The skip.</param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll<TEntity>(int limit, int skip);

        /// <summary>
        /// Finds all the entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="limit">The limit.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll<TEntity>(int limit, int skip, Document orderBy);

        /// <summary>
        /// Finds the specified entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IEnumerable<TEntity> Find<TEntity>(Document conditions);

        /// <summary>
        /// Finds the specified entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IEnumerable<TEntity> Find<TEntity>(Document conditions, Document orderBy);

        /// <summary>
        /// Finds the specified entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="conditions">The conditions.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="skip">The skip.</param>
        /// <returns></returns>
        IEnumerable<TEntity> Find<TEntity>(Document conditions, int limit, int skip);

        /// <summary>
        /// Finds the specified entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="conditions">The conditions.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns></returns>
        IEnumerable<TEntity> Find<TEntity>(Document conditions, int limit, int skip, Document orderBy);

        /// <summary>
        /// Inserts all the entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void InsertAll(IEnumerable<object> entities);

        /// <summary>
        /// Inserts all the entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void InsertAll(params object[] entities);

        /// <summary>
        /// Inserts the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Insert(object entity);

        /// <summary>
        /// Creates a queryable for the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        IQueryable<TEntity> Query<TEntity>();

        /// <summary>
        /// Updates all the entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void UpdateAll(IEnumerable<object> entities);

        /// <summary>
        /// Updates all the entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void UpdateAll(params object[] entities);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(object entity);
    }
}