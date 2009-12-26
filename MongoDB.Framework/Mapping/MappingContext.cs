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
        /// <summary>
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
        /// Gets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public MappingContext Parent { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingContext"/> class.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="owner">The owner.</param>
        public MappingContext(Document document, Type entityType)
        {
            this.Direction = MappingDirection.DocumentToEntity;
            this.Document = document;
            this.Entity = this.CreateEntity(entityType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingContext"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public MappingContext(object entity)
        {
            this.Direction = MappingDirection.EntityToDocument;
            this.Document = new Document();
            this.Entity = entity;
        }

        /// <summary>
        /// Creates a child mapping context.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public MappingContext CreateChildMappingContext(Document document, Type entityType)
        {
            return new MappingContext(document, entityType) { Parent = this };
        }

        /// <summary>
        /// Creates a child mapping context.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public MappingContext CreateChildMappingContext(object entity)
        {
            return new MappingContext(entity) { Parent = this };
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