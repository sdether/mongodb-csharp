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
        private static readonly PropertyInfo instancePropertyInfo = typeof(FluentMap<RootClassMap>).GetProperty("Instance");

        /// <summary>
        /// Adds the maps from assembly containing the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public FluentMappingStore AddMapsFromAssemblyContaining<T>()
        {
            this.AddMapsFromAssembly(typeof(T).Assembly);
            return this;
        }

        /// <summary>
        /// Adds the maps from assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public FluentMappingStore AddMapsFromAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                var baseType = type.BaseType;
                if (baseType.IsGenericType && typeof(FluentRootClassMap<>).IsAssignableFrom(baseType.GetGenericTypeDefinition()))
                {
                    var fluentCollectionMap = Activator.CreateInstance(type);
                    this.AddCollectionMap((RootClassMap)instancePropertyInfo.GetValue(fluentCollectionMap, null));
                }
            }
            return this;
        }

        /// <summary>
        /// Tries to get the missing class map.
        /// </summary>
        /// <param name="type">Type of the entity.</param>
        /// <param name="classMap">The class map.</param>
        /// <returns></returns>
        protected override bool TryGetMissingClassMap(Type type, out ClassMap classMap)
        {
            classMap = null;
            return false;
        }
    }
}