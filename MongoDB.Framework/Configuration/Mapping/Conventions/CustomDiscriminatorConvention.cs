using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public class CustomDiscriminatorConvention : ConventionBase<Type>, IDiscriminatorConvention
    {
        private readonly Func<Type, bool> has;
        private readonly Func<Type, string> key;
        private readonly Func<Type, object> value;

        public CustomDiscriminatorConvention(Func<Type, bool> matcher, Func<Type, bool> has, Func<Type, string> key, Func<Type, object> value)
            : base(matcher)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (value == null)
                throw new ArgumentNullException("value");

            this.key = key;
            this.value = value;
        }

        public string GetDiscriminatorKey(Type type)
        {
            return key(type);
        }

        public object GetDiscriminator(Type type)
        {
            return value(type);
        }

        public bool HasDiscriminator(Type type)
        {
            return has(type);
        }
    }
}