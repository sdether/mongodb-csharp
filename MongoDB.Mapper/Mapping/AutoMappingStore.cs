using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Mapper.Mapping.Auto;

namespace MongoDB.Mapper.Mapping
{
    public class AutoMappingStore : IMappingStore
    {
        private Dictionary<Type, ClassMapBase> autoMaps;
        private IMappingStore mappingStore;
        private IAutoMapper autoMapper;

        public AutoMappingStore()
            : this(new AutoMapper())
        { }

        public AutoMappingStore(IAutoMapper autoMapper)
            : this(autoMapper, null)
        { }

        public AutoMappingStore(IAutoMapper autoMapper, IMappingStore mappingStore)
        {
            if (autoMapper == null)
                throw new ArgumentNullException("autoMapper");

            this.autoMapper = autoMapper;
            this.autoMaps = new Dictionary<Type, ClassMapBase>();
            this.mappingStore = mappingStore ?? new MappingStore();
        }

        public IMongoMapper CreateMongoMapper()
        {
            return new MongoMapper(this);
        }

        public ClassMapBase GetClassMapFor(Type type)
        {
            ClassMapBase classMap = null;
            if (this.autoMaps.TryGetValue(type, out classMap) || this.mappingStore.TryGetClassMapFor(type, out classMap))
                return classMap;

            //automap allowing for recursive calls.  WARNING!!! If any cycles show up, we are hosed...
            classMap = this.autoMapper.CreateClassMap(type, GetClassMapFor);

            this.autoMaps.Add(type, classMap);
            return classMap;
        }

        public bool TryGetClassMapFor(Type type, out ClassMapBase classMap)
        {
            classMap = this.GetClassMapFor(type);
            return true;
        }
    }
}