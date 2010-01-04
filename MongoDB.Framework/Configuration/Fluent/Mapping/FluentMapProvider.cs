using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using MongoDB.Framework.Configuration.Mapping.Models;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public class FluentMapProvider : ModelledMapProvider
    {
        private static readonly PropertyInfo rootModelPropertyInfo = typeof(FluentBase<RootClassMapModel>).GetProperty("Model");
        private static readonly PropertyInfo nestedModelPropertyInfo = typeof(FluentBase<NestedClassMapModel>).GetProperty("Model");

        /// <summary>
        /// Adds the maps from assembly containing the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public FluentMapProvider AddMapsFromAssemblyContaining<T>()
        {
            this.AddMapsFromAssembly(typeof(T).Assembly);
            return this;
        }

        /// <summary>
        /// Adds the maps from assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public FluentMapProvider AddMapsFromAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                var baseType = type.BaseType;
                if (!baseType.IsGenericType)
                    continue;

                if(typeof(FluentRootClass<>).IsAssignableFrom(baseType.GetGenericTypeDefinition()))
                {
                    var fluentRootClassMap = Activator.CreateInstance(type);
                    this.AddRootClassMapModel((RootClassMapModel)rootModelPropertyInfo.GetValue(fluentRootClassMap, null));
                    continue;
                }
                else if (typeof(FluentNestedClass<>).IsAssignableFrom(baseType.GetGenericTypeDefinition()))
                {
                    var fluentNestedClassMap = Activator.CreateInstance(type);
                    this.AddNestedClassMapModel((NestedClassMapModel)nestedModelPropertyInfo.GetValue(fluentNestedClassMap, null));
                    continue;
                }
            }
            return this;
        }

        /// <summary>
        /// Adds the map.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootClassMap">The root class map.</param>
        /// <returns></returns>
        public FluentMapProvider AddMap<T>(FluentRootClass<T> rootClassMap)
        {
            this.AddRootClassMapModel(rootClassMap.Model);
            return this;
        }

        /// <summary>
        /// Adds the map.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nestedClassMap">The nested class map.</param>
        /// <returns></returns>
        public FluentMapProvider AddMap<T>(FluentNestedClass<T> nestedClassMap)
        {
            this.AddNestedClassMapModel(nestedClassMap.Model);
            return this;
        } 

    }
}