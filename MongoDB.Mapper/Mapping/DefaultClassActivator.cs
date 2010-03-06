using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Mapper.Mapping;
using MongoDB.Driver;

namespace MongoDB.Mapper.Mapping
{
    public class DefaultClassActivator : IClassActivator
    {
        public static readonly DefaultClassActivator Instance = new DefaultClassActivator();

        private DefaultClassActivator()
        { }

        public object Activate(Type type, Document document)
        {
            return Activator.CreateInstance(type);
        }
    }
}
