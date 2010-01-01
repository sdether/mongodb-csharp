using System;

namespace MongoDB.Framework.Mapping
{
    public interface IMappingStore
    {
        /// <summary>
        /// Gets the class map for the specified entity type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        ClassMap GetClassMapFor(Type type);

        /// <summary>
        /// Gets the class map for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        ClassMap GetClassMapFor<TEntity>();
    }
}
