using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Mapper.Mapping.IdGenerators;
using MongoDB.Driver;

namespace MongoDB.Mapper.Mapping.Conventions
{
    public class DefaultIdGeneratorConvention : IIdGeneratorConvention
    {
        public static readonly DefaultIdGeneratorConvention Instance = new DefaultIdGeneratorConvention();

        private DefaultIdGeneratorConvention()
        { }

        public IIdGenerator GetGenerator(Type type)
        {
            if (type == typeof(Oid))
                return new MongoDB.Mapper.Mapping.IdGenerators.OidGenerator();

            if (type == typeof(Guid))
                return new GuidCombGenerator();

            return new AssignedGenerator();
        }
    }
}