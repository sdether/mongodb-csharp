using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;

namespace MongoDB.Framework.Hydration
{
    public class EntityHydrator : IEntityHydrator
    {
        #region Private Fields

        private MongoConfiguration configuration;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityHydrator"/> class.
        /// </summary>
        /// <param name="mongoConfiguration">The mongo configuration.</param>
        public EntityHydrator(MongoConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            this.configuration = configuration;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Hydrates the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public TEntity HydrateEntity<TEntity>(Document document)
        {
            var rootEntityMap = this.configuration.GetRootEntityMapFor<TEntity>();
            string id = (string)rootEntityMap.IdMap.GetValueFromDocument(document);
            //Check for cached entity here...

            var entity = Activator.CreateInstance<TEntity>();
            rootEntityMap.IdMap.Setter(entity, id);
            this.Hydrate(rootEntityMap, entity, document);
            return entity;
        }

        /// <summary>
        /// Hydrates the entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="documents">The documents.</param>
        /// <returns></returns>
        public IEnumerable<TEntity> HydrateEntities<TEntity>(IEnumerable<Document> documents)
        {
            foreach (var document in documents)
                yield return this.HydrateEntity<TEntity>(document);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Hydrates the specified entity map.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entityMap">The entity map.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="document">The document.</param>
        private void Hydrate<TEntity>(EntityMap entityMap, TEntity entity, Document document)
        {

        }



        #endregion
    }
}