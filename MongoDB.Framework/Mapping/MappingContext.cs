using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping
{
    public enum MappingDirection
    {
        DocumentToEntity,
        EntityToDocument
    }

    public class MappingContext
    {
        /// Gets the direction of the mapping.
        /// </summary>
        /// <value>The direction.</value>
        public MappingDirection Direction { get; private set; }

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
        /// Gets or sets the mapping store.
        /// </summary>
        /// <value>The mapping store.</value>
        public MappingStore MappingStore { get; private set; }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public MappingContext Parent { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingContext"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="document">The document.</param>
        /// <param name="entityType">Type of the entity.</param>
        public MappingContext(MappingStore mappingStore, Document document, Type entityType)
        {
            if (mappingStore == null)
                throw new ArgumentNullException("mappingStore");
            if (document == null)
                throw new ArgumentNullException("document");
            if (entityType == null)
                throw new ArgumentNullException("entityType");

            this.Direction = MappingDirection.DocumentToEntity;
            
            //use a copy of the document because reading is destructive in order to get the extended properties...
            this.Document = new Document();
            document.CopyTo(this.Document); 

            this.Entity = this.CreateEntity(entityType);
            this.MappingStore = mappingStore;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingContext"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="entity">The entity.</param>
        public MappingContext(MappingStore mappingStore, object entity)
        {
            if (mappingStore == null)
                throw new ArgumentNullException("mappingStore");
            if (entity == null)
                throw new ArgumentNullException("entity");

            this.Direction = MappingDirection.EntityToDocument;
            this.Document = new Document();
            this.Entity = entity;
            this.MappingStore = mappingStore;
        }

        /// <summary>
        /// Creates a child mapping context.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public MappingContext CreateChildMappingContext(Document document, Type entityType)
        {
            return new MappingContext(this.MappingStore, document, entityType) { Parent = this };
        }

        /// <summary>
        /// Creates a child mapping context.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public MappingContext CreateChildMappingContext(object entity)
        {
            return new MappingContext(this.MappingStore, entity) { Parent = this };
        }

        /// <summary>
        /// Creates the entity.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        protected virtual object CreateEntity(Type entityType)
        {
            return Activator.CreateInstance(entityType);
        }
    }
}