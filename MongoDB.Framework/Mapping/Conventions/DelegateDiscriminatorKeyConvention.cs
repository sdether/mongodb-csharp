using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Conventions
{
    public class DelegateDiscriminatorKeyConvention : IDiscriminatorKeyConvention
    {
        private Func<Type, string> key;

        public DelegateDiscriminatorKeyConvention(Func<Type, string> key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            this.key = key;
        }

        public string GetDiscriminatorKey(Type type)
        {
            return key(type);
        }
    }
}
