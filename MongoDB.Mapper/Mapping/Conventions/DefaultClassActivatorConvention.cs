using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Mapping.Conventions
{
    public class DefaultClassActivatorConvention : IClassActivatorConvention
    {
        public static readonly DefaultClassActivatorConvention Instance = new DefaultClassActivatorConvention();

        private DefaultClassActivatorConvention()
        { }

        public IClassActivator GetClassActivator(Type type)
        {
            return DefaultClassActivator.Instance;
        }
    }
}
