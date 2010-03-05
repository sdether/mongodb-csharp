using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Auto
{
    public class AggregateAutoMapper : IAutoMapper
    {
        private List<IAutoMapper> autoMappers;

        public AggregateAutoMapper()
        {
            this.autoMappers = new List<IAutoMapper>();
        }

        public void AddAutoMapper(IAutoMapper autoMapper)
        {
            if (autoMapper == null)
                throw new ArgumentNullException("autoMapper");

            this.autoMappers.Add(autoMapper);
        }

        public bool CanCreateClassMap(Type type)
        {
            return this.autoMappers.Any(x => x.CanCreateClassMap(type));
        }

        public ClassMapBase CreateClassMap(Type type, Func<Type, ClassMapBase> existingClassMapFinder)
        {
            var autoMapper = this.autoMappers.FirstOrDefault(x => x.CanCreateClassMap(type));
            if (autoMapper == null)
                throw new InvalidOperationException(string.Format("Cannot map type {0}. Ensure a call to CanCreateClassMap to avoid this exception.", type));

            return autoMapper.CreateClassMap(type, existingClassMapFinder);
        }
    }
}