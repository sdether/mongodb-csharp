using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Mapping.Conventions
{
    public interface IDiscriminatorKeyConvention
    {
        /// <summary>
        /// Gets the discriminator key.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        string GetDiscriminatorKey(Type type);
    }
}