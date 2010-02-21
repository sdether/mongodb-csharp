using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Reflection;
using MongoDB.Framework.Configuration.Fluent.Mapping.Conventions;
using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public class FluentMapModelRegistry : IMapModelRegistry
    {
        #region Private Static Fields

        private static readonly PropertyInfo rootModelPropertyInfo = typeof(FluentBase<RootClassMapModel>).GetProperty("Model");
        private static readonly PropertyInfo nestedModelPropertyInfo = typeof(FluentBase<NestedClassMapModel>).GetProperty("Model");
        private static readonly PropertyInfo subModelPropertyInfo = typeof(FluentBase<SubClassMapModel>).GetProperty("Model");

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
        /// Adds the model.
        /// </summary>
        /// <param name="model">The model.</param>
        public FluentMapModelRegistry AddModel(RootClassMapModel model)
        {
            this.registry.AddModel(model);
            return this;
        }

        /// <summary>
        /// Adds the nested class map model.
        /// </summary>
        /// <param name="nestedClassMapModel">The nested class map model.</param>
        public FluentMapModelRegistry AddModel(NestedClassMapModel model)
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
        /// <param name="rootClassMap">The root class map.</param>
        /// <returns></returns>
        public FluentMapModelRegistry AddMap<T>(FluentRootClass<T> rootClassMap)
        {
            this.AddModel(rootClassMap.Model);
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
            this.AddModel(nestedClassMap.Model);
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

                if (typeof(FluentRootClass<>).IsAssignableFrom(genDef))
                {
                    var fluentRootClassMap = Activator.CreateInstance(type);
                    this.AddModel((RootClassMapModel)rootModelPropertyInfo.GetValue(fluentRootClassMap, null));
                }
                else if (typeof(FluentNestedClass<>).IsAssignableFrom(genDef))
                {
                    var fluentNestedClassMap = Activator.CreateInstance(type);
                    this.AddModel((NestedClassMapModel)nestedModelPropertyInfo.GetValue(fluentNestedClassMap, null));
                }
                else if (typeof(FluentSubClass<>).IsAssignableFrom(genDef))
                {
                    var fluentSubClassMap = Activator.CreateInstance(type);
                    this.AddModel((SubClassMapModel)subModelPropertyInfo.GetValue(fluentSubClassMap, null));
                }
            }
            return this;
        }

        public FluentMapModelRegistry AddSource(IClassMapModelSource source)
        {
            this.registry.AddSource(source);
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