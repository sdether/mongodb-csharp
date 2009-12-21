using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;

namespace MongoDB.Framework.Hydration
{
    public interface IEntityHydrator
    {
        /// <summary>
        /// Hydrates the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        TEntity HydrateEntity<TEntity>(Document document);

        /// <summary>
        /// Hydrates the entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="documents">The documents.</param>
        /// <returns></returns>
        IEnumerable<TEntity> HydrateEntities<TEntity>(IEnumerable<Document> documents);
    }
}