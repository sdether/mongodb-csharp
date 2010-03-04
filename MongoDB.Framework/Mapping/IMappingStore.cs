using System;
using System.Collections.Generic;

namespace MongoDB.Framework.Mapping
{
    public interface IMappingStore
    {
        /// <summary>
        /// Creates the mongo mapper.
        /// </summary>
        /// <returns></returns>
        IMongoMapper CreateMongoMapper();

        /// <summary>
        /// Gets the class map for the specified entity type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        ClassMapBase GetClassMapFor(Type type);

        /// <summary>
        /// Tries the get class map for.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="classMap">The class map.</param>
        /// <returns></returns>
        bool TryGetClassMapFor(Type type, out ClassMapBase classMap);
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

        /// <summary>
        /// Tries the get class map for.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="classMap">The class map.</param>
        /// <returns></returns>
        public static bool TryGetClassMapFor<TEntity>(this IMappingStore mappingStore, out ClassMapBase classMap)
        {
            return mappingStore.TryGetClassMapFor(typeof(TEntity), out classMap);
        }

    }
}
