using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public class NullExtendedPropertiesConvention : ConventionBase<Type>, IExtendedPropertiesConvention
    {
        public static readonly NullExtendedPropertiesConvention AlwaysMatching = new NullExtendedPropertiesConvention();

        private NullExtendedPropertiesConvention()
            : base(t => true)
        { }

        public ExtendedPropertiesMapModel GetExtendedPropertiesMapModel(Type type)
        {
            throw new NotSupportedException();
        }

        public bool HasExtendedProperties(Type type)
        {
            return false;
        }
    }
}