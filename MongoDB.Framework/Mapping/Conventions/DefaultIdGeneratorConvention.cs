using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.IdGenerators;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping.Conventions
{
    public class DefaultIdGeneratorConvention : IIdGeneratorConvention
    {
        public static readonly DefaultIdGeneratorConvention Instance = new DefaultIdGeneratorConvention();

        private DefaultIdGeneratorConvention()
        { }

        public IIdGenerator GetGenerator(Type type)
        {
            if (type == typeof(Oid))
                return new MongoDB.Framework.Mapping.IdGenerators.OidGenerator();

            if (type == typeof(Guid))
                return new GuidCombGenerator();

            return new AssignedGenerator();
        }
    }
}