using System;

namespace MongoDB.Framework.Configuration.Mapping
{
    public interface IMappingStore
    {
        /// <summary>
        /// Gets the class map for the specified entity type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        ClassMap GetClassMapFor(Type type);
    }

    public static class IMappingStoreExtensions
    {
        /// <summary>
        /// Gets the class map for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="mappingStore">The mapping store.</param>
        /// <returns></returns>
        public static ClassMap GetClassMapFor<TEntity>(this IMappingStore mappingStore)
        {
            return mappingStore.GetClassMapFor(typeof(TEntity));
        }
    }
}
