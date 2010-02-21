using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Auto
{
    public static class AutoMap
    {
        public static AutoPersistenceModel FromAssemblyOf<T>()
        {
            return FromAssembly(typeof(T).Assembly);
        }

        public static AutoPersistenceModel FromAssembly(Assembly assembly)
        {
            return FromSource(new AssemblyTypeSource(assembly));
        }

        public static AutoPersistenceModel FromSource(ITypeSource source)
        {
            var model = new AutoPersistenceModel();
            model.AddTypeSource(source);

            return model;
        }

        public static AutoPersistenceModel Type<T>()
        {
            return FromSource(new SingleTypeSource(typeof(T)));
        }
    }
}