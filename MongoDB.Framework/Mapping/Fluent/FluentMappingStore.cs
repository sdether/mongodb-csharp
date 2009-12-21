using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentMappingStore : MappingStore
    {
        private static readonly PropertyInfo instancePropertyInfo = typeof(FluentMap<CollectionMap>).GetProperty("Instance");

        /// <summary>
        /// Gets the maps from assembly containing.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void GetMapsFromAssemblyContaining<T>()
        {
            this.GetMapsFromAssembly(typeof(T).Assembly);
        }

        /// <summary>
        /// Gets the maps from assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void GetMapsFromAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                var baseType = type.BaseType;
                if (baseType.IsGenericType && typeof(FluentCollectionMap<>).IsAssignableFrom(baseType.GetGenericTypeDefinition()))
                {
                    var fluentCollectionMap = Activator.CreateInstance(type);
                    this.AddCollectionMap((CollectionMap)instancePropertyInfo.GetValue(fluentCollectionMap, null));
                }
            }
        }

        /// <summary>
        /// Tries to get the missing document map.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="documentMap">The document map.</param>
        /// <returns></returns>
        protected override bool TryGetMissingDocumentMap(Type entityType, out DocumentMap documentMap)
        {
            documentMap = null;
            return false;
        }
    }
}