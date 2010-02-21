using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping.Auto
{
    public class SingleTypeSource : ITypeSource
    {
        private readonly Type type;

        public SingleTypeSource(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            this.type = type;
        }

        public IEnumerable<Type> GetTypes()
        {
            yield return type;
        }
    }
}
