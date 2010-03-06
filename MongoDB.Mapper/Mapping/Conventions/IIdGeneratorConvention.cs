using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Mapping.Conventions
{
    public interface IIdGeneratorConvention
    {
        IIdGenerator GetGenerator(Type type);
    }
}