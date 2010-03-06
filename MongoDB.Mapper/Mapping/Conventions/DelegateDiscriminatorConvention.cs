using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Mapping.Conventions
{
    public class DelegateDiscriminatorConvention : IDiscriminatorConvention
    {
        private Func<Type, object> discriminator;

        public DelegateDiscriminatorConvention(Func<Type, object> discriminator)
        {
            if (discriminator == null)
                throw new ArgumentNullException("discriminator");

            this.discriminator = discriminator;
        }

        public object GetDiscriminator(Type type)
        {
            return discriminator(type);
        }
    }
}
