using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Configuration.Fluent.Mapping;
using MongoDB.Framework.Configuration.Mapping.Conventions;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Auto
{
    public class AutoPersistenceModel : IClassMapModelSource
    {
        public readonly static MappingConventions DefaultConventions = new MappingConventions()
        {
            IdConvention = new NamedIdConvention(t => true, "Id"),
            ExtendedPropertiesConvention = new NamedExtendedPropertiesConvention(t => true, "ExtendedProperties"),
            MemberFinder = new CustomMemberFinder(t => true),
        };

        private MappingConventions conventions;
        private HashSet<Type> includeTypes;
        private HashSet<Type> excludeTypes;
        private Func<Type, bool> typeFilter;
        private List<ITypeSource> typeSources;

        private AutoMappingExpressions expressions;

        public AutoPersistenceModel()
        {
            conventions = DefaultConventions;
            this.expressions = new AutoMappingExpressions();
            this.excludeTypes = new HashSet<Type>();
            this.includeTypes = new HashSet<Type>();
            this.typeSources = new List<ITypeSource>();
        }

        public AutoPersistenceModel AddTypeSource(ITypeSource typeSource)
        {
            if (typeSource == null)
	            throw new ArgumentNullException("typeSource");

            this.typeSources.Add(typeSource);
            return this;
        }

        public AutoPersistenceModel AddAssemblyOf<T>()
        {
            return this.AddAssembly(typeof(T).Assembly);
        }

        public AutoPersistenceModel AddAssembly(Assembly assembly)
        {
            this.AddTypeSource(new AssemblyTypeSource(assembly));
            return this;
        }

        public IEnumerable<ClassMapModel> GetClassMapModels()
        {
            foreach (var type in typeSources.SelectMany(x => x.GetTypes()))
            {
                if(this.typeFilter != null && !this.typeFilter(type))
                    continue;

                if(!this.ShouldMap(type))
                    continue;

                if (this.expressions.IsSubClass(type))
                    yield return new SubClassMapModel(type)
                    {
                        Conventions = this.conventions,
                        Discriminator = this.expressions.DiscriminatorValue(type)
                    };
                else if (this.expressions.IsNestedClass(type))
                    yield return new NestedClassMapModel(type) 
                    { 
                        Conventions = this.conventions,
                        DiscriminatorKey = this.expressions.DiscriminatorKey(type)
                    };
                else if(this.expressions.IsRootClass(type))
                    yield return new RootClassMapModel(type) 
                    { 
                        Conventions = this.conventions,
                        DiscriminatorKey = this.expressions.DiscriminatorKey(type)
                    };
                
            }
        }

        public AutoPersistenceModel ExcludeType(Type type)
        {
            this.excludeTypes.Add(type);
            return this;
        }

        public AutoPersistenceModel ExcluedType<T>()
        {
            return this.ExcludeType(typeof(T));
        }

        public AutoPersistenceModel IncludeType(Type type)
        {
            this.includeTypes.Add(type);
            return this;
        }

        public AutoPersistenceModel IncludeType<T>()
        {
            return this.IncludeType(typeof(T));
        }

        public AutoPersistenceModel SetupConventions(Action<FluentConventions> config)
        {
            return this.SetupConventions(this.conventions, config);
        }

        public AutoPersistenceModel SetupConventions(MappingConventions baseConventions, Action<FluentConventions> config)
        {
            var copy = baseConventions.Copy();
            config(new FluentConventions(copy));
            this.conventions = copy;
            return this;
        }

        public AutoPersistenceModel SetupExpressions(Action<AutoMappingExpressions> config)
        {
            config(this.expressions);
            return this;
        }

        public AutoPersistenceModel UseConventions(MappingConventions conventions)
        {
            this.conventions = conventions;
            return this;
        }

        public AutoPersistenceModel Where(Func<Type, bool> typeFilter)
        {
            this.typeFilter = typeFilter;
            return this;
        }

        private bool ShouldMap(Type type)
        {
            if (this.includeTypes.Contains(type))
                return true;
            if (this.excludeTypes.Contains(type))
                return false;
            if (type.IsGenericType && this.excludeTypes.Contains(type.GetGenericTypeDefinition()))
                return false;
            if (type.IsAbstract)
                return false;
            if (type == typeof(object))
                return false;

            return true;
        }
    }
}