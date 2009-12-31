using System;
using MongoDB.Driver;
namespace MongoDB.Framework.Mapping
{
    public interface IMappingContext
    {
        /// <summary>
        /// Gets the document.
        /// </summary>
        /// <value>The document.</value>
        Document Document { get; }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <value>The entity.</value>
        object Entity { get; }

        /// <summary>
        /// Gets the mongo context.
        /// </summary>
        /// <value>The mongo context.</value>
        IMongoContext MongoContext { get; }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>The parent.</value>
        IMappingContext Parent { get; }

        /// <summary>
        /// Creates the child mapping context.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns></returns>
        IMappingContext CreateChildMappingContext(Document document, object entity);
    }
}
