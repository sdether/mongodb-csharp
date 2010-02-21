using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Auto
{
    public class AutoMappingExpressions
    {
        public Func<Type, bool> IsNestedClass = t => false;
        public Func<Type, bool> IsSubClass;
        public Func<Type, bool> IsRootClass = t => true;

        public Func<Type, string> DiscriminatorKey = t => "type";
        public Func<Type, string> DiscriminatorValue = t => t.Name;

        public AutoMappingExpressions()
        {
            IsSubClass = t => IsNestedClass(t.BaseType) || IsRootClass(t.BaseType);
        }
    }
}