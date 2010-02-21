using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Auto
{
    public class AssemblyTypeSource : ITypeSource
    {
        private readonly Assembly assembly;

        public AssemblyTypeSource(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            this.assembly = assembly;
        }

        public IEnumerable<Type> GetTypes()
        {
            return this.assembly.GetExportedTypes();
        }
    }
}