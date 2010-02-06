using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public class FluentMapModelRegistry : MapModelRegistry
    {
        private static readonly PropertyInfo rootModelPropertyInfo = typeof(FluentBase<RootClassMapModel>).GetProperty("Model");
        private static readonly PropertyInfo nestedModelPropertyInfo = typeof(FluentBase<NestedClassMapModel>).GetProperty("Model");
        private static readonly PropertyInfo subModelPropertyInfo = typeof(FluentBase<SubClassMapModel>).GetProperty("Model");

        /// <summary>
        /// Adds the maps from assembly containing the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public FluentMapModelRegistry AddMapsFromAssemblyContaining<T>()
        {
            this.AddMapsFromAssembly(typeof(T).Assembly);
            return this;
        }

        /// <summary>
        /// Adds the maps from assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public FluentMapModelRegistry AddMapsFromAssembly(Assembly assembly)
        {
            var types = from t in assembly.GetTypes()
                        where !t.IsInterface
                            && !t.IsAbstract
                            && t.BaseType != null
                            && t.BaseType.IsGenericType
                        select t;

            foreach (var type in types)
            {
                var genDef = type.BaseType.GetGenericTypeDefinition();

                if (typeof(FluentRootClass<>).IsAssignableFrom(genDef))
                {
                    var fluentRootClassMap = Activator.CreateInstance(type);
                    this.AddRootClassMapModel((RootClassMapModel)rootModelPropertyInfo.GetValue(fluentRootClassMap, null));
                }
                else if (typeof(FluentNestedClass<>).IsAssignableFrom(genDef))
                {
                    var fluentNestedClassMap = Activator.CreateInstance(type);
                    this.AddNestedClassMapModel((NestedClassMapModel)nestedModelPropertyInfo.GetValue(fluentNestedClassMap, null));
                }
                else if (typeof(FluentSubClass<>).IsAssignableFrom(genDef))
                {
                    var fluentSubClassMap = Activator.CreateInstance(type);
                    this.AddSubClassMapModel((SubClassMapModel)subModelPropertyInfo.GetValue(fluentSubClassMap, null));
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
        public FluentMapModelRegistry AddMap<T>(FluentRootClass<T> rootClassMap)
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
        public FluentMapModelRegistry AddMap<T>(FluentNestedClass<T> nestedClassMap)
        {
            this.AddNestedClassMapModel(nestedClassMap.Model);
            return this;
        }

        /// <summary>
        /// Adds the map.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subClassMap">The sub class map.</param>
        /// <returns></returns>
        public FluentMapModelRegistry AddMap<T>(FluentSubClass<T> subClassMap)
        {
            this.AddSubClassMapModel(subClassMap.Model);
            return this;
        }

    }
}