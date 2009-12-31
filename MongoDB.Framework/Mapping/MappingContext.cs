using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Framework.Configuration;

namespace MongoDB.Framework.Mapping
{
    public class MappingContext : IMappingContext
    {
        /// <summary>
        /// Gets the document.
        /// </summary>
        /// <value>The document.</value>
        public Document Document { get; private set; }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <value>The owner.</value>
        public object Entity { get; private set; }

        /// <summary>
        /// Gets the mongo context.
        /// </summary>
        /// <value>The mongo context.</value>
        public IMongoContext MongoContext { get; private set; }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public IMappingContext Parent { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingContext"/> class.
        /// </summary>
        /// <param name="mongoContext">The mongo context.</param>
        /// <param name="document">The document.</param>
        /// <param name="entity">The entity.</param>
        public MappingContext(IMongoContext mongoContext, Document document, object entity)
        {
            if (mongoContext == null)
                throw new ArgumentNullException("mongoContext");
            if (document == null)
                throw new ArgumentNullException("document");
            if (entity == null)
                throw new ArgumentNullException("entity");

            this.Document = document;
            this.Entity = entity;
            this.MongoContext = mongoContext;
        }

        /// <summary>
        /// Creates a child mapping context.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public IMappingContext CreateChildMappingContext(Document document, object entity)
        {
            if (document == null)
                throw new ArgumentNullException("document");
            if (entity == null)
                throw new ArgumentNullException("entity");
            return new MappingContext(this.MongoContext, document, entity) { Parent = this };
        }

    }
}