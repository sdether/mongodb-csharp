using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Reflection;
using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public class FluentMapModelRegistry : IMapModelRegistry
    {
        #region Private Static Fields

        private static readonly PropertyInfo classModelPropertyInfo = typeof(FluentBase<ClassMapModel>).GetProperty("Model");
        private static readonly PropertyInfo subClassModelPropertyInfo = typeof(FluentBase<SubClassMapModel>).GetProperty("Model");

        #endregion

        #region Private Fields

        private MapModelRegistry registry;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentMapModelRegistry"/> class.
        /// </summary>
        public FluentMapModelRegistry()
        {
            this.registry = new MapModelRegistry();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentMapModelRegistry"/> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        public FluentMapModelRegistry(MapModelRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException("registry");

            this.registry = registry;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the nested class map model.
        /// </summary>
        /// <param name="nestedClassMapModel">The nested class map model.</param>
        public FluentMapModelRegistry AddModel(ClassMapModel model)
        {
            this.registry.AddModel(model);
            return this;
        }

        /// <summary>
        /// Adds the sub class map model.
        /// </summary>
        /// <param name="subClassMapModel">The sub class map model.</param>
        public FluentMapModelRegistry AddModel(SubClassMapModel model)
        {
            this.registry.AddModel(model);
            return this;
        }

        /// <summary>
        /// Adds the map.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="classMap">The class map.</param>
        /// <returns></returns>
        public FluentMapModelRegistry AddMap<T>(FluentClass<T> classMap)
        {
            this.AddModel(classMap.Model);
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
            this.AddModel(subClassMap.Model);
            return this;
        }

        /// <summary>
        /// Adds the maps from assembly containing.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public FluentMapModelRegistry AddMapsFromAssemblyContaining<T>()
        {
            return this.AddMapsFromAssembly(typeof(T).Assembly);
        }

        /// <summary>
        /// Adds the maps from assembly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public FluentMapModelRegistry AddMapsFromAssembly(Assembly assembly)
        {
            var types = from t in assembly.GetTypes()
                        where t.IsClass
                            && !t.IsAbstract
                            && t.BaseType != null
                            && t.BaseType.IsGenericType
                        select t;

            foreach (var type in types)
            {
                var genDef = type.BaseType.GetGenericTypeDefinition();

                if (typeof(FluentClass<>).IsAssignableFrom(genDef))
                {
                    var fluentRootClassMap = Activator.CreateInstance(type);
                    this.AddModel((ClassMapModel)classModelPropertyInfo.GetValue(fluentRootClassMap, null));
                }
                else if (typeof(FluentSubClass<>).IsAssignableFrom(genDef))
                {
                    var fluentSubClassMap = Activator.CreateInstance(type);
                    this.AddModel((SubClassMapModel)subClassModelPropertyInfo.GetValue(fluentSubClassMap, null));
                }
            }
            return this;
        }

        /// <summary>
        /// Builds the mapping store.
        /// </summary>
        /// <returns></returns>
        public IMappingStore BuildMappingStore()
        {
            return this.registry.BuildMappingStore();
        }

        #endregion
    }
}