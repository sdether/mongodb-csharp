using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class MappingStore : IMappingStore
    {
        private Dictionary<Type, ClassMapBase> classMaps;

        /// <summary>
        /// Initializes a new instance of the <see cref="IMappingStore"/> class.
        /// </summary>
        public MappingStore()
            : this(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IMappingStore"/> class.
        /// </summary>
        /// <param name="classMaps">The class maps.</param>
        public MappingStore(IEnumerable<ClassMap> classMaps)
        {
            this.classMaps = new Dictionary<Type, ClassMapBase>();
            if (classMaps != null)
            {
                foreach (var classMap in classMaps)
                    this.AddClassMap(classMap);
            }
        }

        /// <summary>
        /// Adds the class map.
        /// </summary>
        /// <param name="classMap">The class map.</param>
        public void AddClassMap(ClassMap classMap)
        {
            if (classMap == null)
                throw new ArgumentNullException("classMap");

            this.classMaps.Add(classMap.Type, classMap);
            foreach (var subClassMap in classMap.SubClassMaps)
                this.classMaps.Add(subClassMap.Type, subClassMap);
        }

        /// <summary>
        /// Creates the mongo mapper.
        /// </summary>
        /// <returns></returns>
        public IMongoMapper CreateMongoMapper()
        {
            return new MongoMapper(this);
        }

        /// <summary>
        /// Gets the class map for.
        /// </summary>
        /// <param name="type">ValueType of the entity.</param>
        /// <returns></returns>
        public ClassMapBase GetClassMapFor(Type type)
        {
            ClassMapBase classMap = null;
            if (!this.classMaps.TryGetValue(type, out classMap))
                throw new UnmappedTypeException(string.Format("The type {0} is unmapped.", type));

            return classMap;
        }

        /// <summary>
        /// Tries the get class map for.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="classMap">The class map.</param>
        /// <returns></returns>
        public bool TryGetClassMapFor(Type type, out ClassMapBase classMap)
        {
            return this.classMaps.TryGetValue(type, out classMap);
        }        
    }
}