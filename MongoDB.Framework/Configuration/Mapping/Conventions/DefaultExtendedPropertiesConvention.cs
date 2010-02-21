using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public class DefaultExtendedPropertiesConvention : ConventionBase<Type>, IExtendedPropertiesConvention
    {
        public static readonly DefaultExtendedPropertiesConvention AlwaysMatching = new DefaultExtendedPropertiesConvention();

        private DefaultExtendedPropertiesConvention()
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