using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Mapping.Conventions
{
    public interface IDiscriminatorConvention
    {
        object GetDiscriminator(Type type);
    }
}