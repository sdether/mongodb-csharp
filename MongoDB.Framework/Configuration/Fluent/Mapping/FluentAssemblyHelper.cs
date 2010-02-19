using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Configuration.Fluent.Mapping.Auto;
using MongoDB.Framework.Configuration.Mapping;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public class FluentAssemblyHelper
    {
        private static readonly PropertyInfo rootModelPropertyInfo = typeof(FluentBase<RootClassMapModel>).GetProperty("Model");
        private static readonly PropertyInfo nestedModelPropertyInfo = typeof(FluentBase<NestedClassMapModel>).GetProperty("Model");
        private static readonly PropertyInfo subModelPropertyInfo = typeof(FluentBase<SubClassMapModel>).GetProperty("Model");

        public Assembly Assembly { get; private set; }

        public FluentMapModelRegistry Registry { get; private set; }

        public FluentAssemblyHelper(FluentMapModelRegistry registry, Assembly assembly)
        {
            if (registry == null)
                throw new ArgumentNullException("registry");
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            this.Assembly = assembly;
            this.Registry = registry;
        }

        /// <summary>
        /// Adds the maps from assembly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public FluentMapModelRegistry AddMaps()
        {
            var types = from t in this.Assembly.GetTypes()
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
                    this.Registry.AddRootClassMapModel((RootClassMapModel)rootModelPropertyInfo.GetValue(fluentRootClassMap, null));
                }
                else if (typeof(FluentNestedClass<>).IsAssignableFrom(genDef))
                {
                    var fluentNestedClassMap = Activator.CreateInstance(type);
                    this.Registry.AddNestedClassMapModel((NestedClassMapModel)nestedModelPropertyInfo.GetValue(fluentNestedClassMap, null));
                }
                else if (typeof(FluentSubClass<>).IsAssignableFrom(genDef))
                {
                    var fluentSubClassMap = Activator.CreateInstance(type);
                    this.Registry.AddSubClassMapModel((SubClassMapModel)subModelPropertyInfo.GetValue(fluentSubClassMap, null));
                }
            }
            return this.Registry;
        }

        public FluentMapModelRegistry AutoMapTypesOf<T>(Action<FluentAutoMap> config)
        {
            var types = from t in this.Assembly.GetTypes()
                        where !t.IsInterface
                            && !t.IsAbstract
                            && t.BaseType != null
                        select t;

            foreach (var type in types)
            {
                //new FluentAutoMap().
            }

            return this.Registry;
        }

    }
}