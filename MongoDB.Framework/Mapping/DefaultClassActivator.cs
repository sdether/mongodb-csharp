using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping
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
