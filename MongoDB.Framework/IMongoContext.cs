using System;
using System.Collections.Generic;
using System.Linq;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;

namespace MongoDB.Framework
{
    public interface IMongoContext : IDisposable
    {
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        IMongoConfiguration Configuration { get; }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>The database.</value>
        Database Database { get; }

        /// <summary>
        /// Deletes all the entities on submit.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void DeleteAllOnSubmit(params object[] entities);

        /// <summary>
        /// Deletes all the entities on submit.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void DeleteAllOnSubmit(IEnumerable<object> entities);

        /// <summary>
        /// Deletes the entity on submit.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void DeleteOnSubmit(object entity);

        /// <summary>
        /// Inserts all the entities on submit.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void InsertAllOnSubmit(IEnumerable<object> entities);

        /// <summary>
        /// Inserts all the entities on submit.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void InsertAllOnSubmit(params object[] entities);

        /// <summary>
        /// Inserts the entity on submit.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void InsertOnSubmit(object entity);

        /// <summary>
        /// Creates a queryable for the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        IQueryable<TEntity> Query<TEntity>();

        /// <summary>
        /// Submits the changes to the database.
        /// </summary>
        void SubmitChanges();
    }
}
