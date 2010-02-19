using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public class NullDiscriminatorConvention : ConventionBase<Type>, IDiscriminatorConvention
    {
        public static readonly NullDiscriminatorConvention AlwaysMatching = new NullDiscriminatorConvention();

        private NullDiscriminatorConvention()
            : base(t => true)
        { }

        public string GetDiscriminatorKey(Type type)
        {
            throw new NotSupportedException();
        }

        public object GetDiscriminator(Type type)
        {
            throw new NotSupportedException();
        }

        public bool HasDiscriminator(Type type)
        {
            return false;
        }
    }
}