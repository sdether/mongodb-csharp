using System;
using System.Collections.Generic;

namespace MongoDB.Framework.Mapping
{
    public interface IMappingStore
    {
        /// <summary>
        /// Gets the class maps.
        /// </summary>
        /// <value>The class maps.</value>
        IEnumerable<ClassMap> ClassMaps { get; }

        /// <summary>
        /// Gets the class map for the specified entity type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        ClassMapBase GetClassMapFor(Type type);

        /// <summary>
        /// Creates the mongo mapper.
        /// </summary>
        /// <returns></returns>
        IMongoMapper CreateMongoMapper();
    }

    public static class IMappingStoreExtensions
    {
        /// <summary>
        /// Gets the class map for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="mappingStore">The mapping store.</param>
        /// <returns></returns>
        public static ClassMapBase GetClassMapFor<TEntity>(this IMappingStore mappingStore)
        {
            return mappingStore.GetClassMapFor(typeof(TEntity));
        }
    }
}
