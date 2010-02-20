using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Configuration.Fluent.Mapping.Auto;
using MongoDB.Framework.Configuration.Mapping;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public class FluentAssembly
    {
        private static readonly PropertyInfo rootModelPropertyInfo = typeof(FluentBase<RootClassMapModel>).GetProperty("Model");
        private static readonly PropertyInfo nestedModelPropertyInfo = typeof(FluentBase<NestedClassMapModel>).GetProperty("Model");
        private static readonly PropertyInfo subModelPropertyInfo = typeof(FluentBase<SubClassMapModel>).GetProperty("Model");

        private Assembly assembly;
        private FluentMapModelRegistry registry;

        public FluentAssembly(FluentMapModelRegistry registry, Assembly assembly)
        {
            if (registry == null)
                throw new ArgumentNullException("registry");
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            this.assembly = assembly;
            this.registry = registry;
        }

        /// <summary>
        /// Adds the maps from assembly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public FluentMapModelRegistry AddMaps()
        {
            var types = from t in this.assembly.GetTypes()
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
                    this.registry.AddRootClassMapModel((RootClassMapModel)rootModelPropertyInfo.GetValue(fluentRootClassMap, null));
                }
                else if (typeof(FluentNestedClass<>).IsAssignableFrom(genDef))
                {
                    var fluentNestedClassMap = Activator.CreateInstance(type);
                    this.registry.AddNestedClassMapModel((NestedClassMapModel)nestedModelPropertyInfo.GetValue(fluentNestedClassMap, null));
                }
                else if (typeof(FluentSubClass<>).IsAssignableFrom(genDef))
                {
                    var fluentSubClassMap = Activator.CreateInstance(type);
                    this.registry.AddSubClassMapModel((SubClassMapModel)subModelPropertyInfo.GetValue(fluentSubClassMap, null));
                }
            }
            return this.registry;
        }

        public FluentMapModelRegistry AutoMapAsRootClassesTypesDerivedFrom<T>(Action<FluentAutoMap> config)
        {
            var types = from t in this.assembly.GetTypes()
                        where !t.IsInterface
                            && !t.IsAbstract
                            && t.BaseType != null
                        select t;

            var fluentAutoMap = new FluentAutoMap(new AutoMapModel());
            config(fluentAutoMap);

            foreach (var type in types)
            {
                var model = new RootClassMapModel(type);
                model.AutoMap = fluentAutoMap.Model;
                this.registry.AddRootClassMapModel(model);
            }

            return this.registry;
        }

        public FluentMapModelRegistry AutoMapAsSubClassesTypesDerivedFrom<T>(Action<FluentAutoMap> config)
        {
            var types = from t in this.assembly.GetTypes()
                        where !t.IsInterface
                            && !t.IsAbstract
                            && t.BaseType != null
                        select t;

            var fluentAutoMap = new FluentAutoMap(new AutoMapModel());
            config(fluentAutoMap);

            foreach (var type in types)
            {
                var model = new SubClassMapModel(type);
                model.AutoMap = fluentAutoMap.Model;
                this.registry.AddSubClassMapModel(model);
            }

            return this.registry;
        }

    }
}