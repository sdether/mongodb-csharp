using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Configuration.Visitors;

namespace MongoDB.Framework
{
    public class EntityMapper
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public MongoConfiguration Configuration { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMapper"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public EntityMapper(MongoConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            this.Configuration = configuration;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Maps the document to entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public object MapDocumentToEntity(Document document, Type entityType)
        {
            if (document == null)
                throw new ArgumentNullException("document");
            if (entityType == null)
                throw new ArgumentNullException("entityType");

            var translator = new DocumentToEntityTranslator(document);
            var rootEntityMap = this.Configuration.GetRootEntityMapFor(entityType);
            rootEntityMap.Accept(translator);
            return translator.Entity;
        }

        /// <summary>
        /// Maps the entity to document.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public Document MapEntityToDocument(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var translator = new EntityToDocumentTranslator(entity);
            var rootEntityMap = this.Configuration.GetRootEntityMapFor(entity.GetType());
            rootEntityMap.Accept(translator);
            return translator.Document;
        }

        #endregion
    }
}