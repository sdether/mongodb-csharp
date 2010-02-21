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

        public static AutoPersistenceModel FromAssemblyOf<T>(Func<Type, bool> filter)
        {
            return FromAssembly(typeof(T).Assembly, filter);
        }

        public static AutoPersistenceModel FromAssembly(Assembly assembly)
        {
            return FromAssembly(assembly, null);
        }

        public static AutoPersistenceModel FromAssembly(Assembly assembly, Func<Type, bool> filter)
        {
            return FromSource(new AssemblyTypeSource(assembly), filter);
        }

        public static AutoPersistenceModel FromSource(ITypeSource source)
        {
            return FromSource(source, null);
        }

        public static AutoPersistenceModel FromSource(ITypeSource source, Func<Type, bool> filter)
        {
            var model = new AutoPersistenceModel();
            model.AddTypeSource(source);
            if(filter != null)
                model.Where(filter);

            return model;
        }

        public static AutoPersistenceModel Type<T>()
        {
            return FromSource(new SingleTypeSource(typeof(T)), null);
        }
    }
}