using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public class CamelCaseCollectionNameConvention : ConventionBase<Type>, ICollectionNameConvention
    {
        public static readonly CamelCaseCollectionNameConvention AlwaysMatching = new CamelCaseCollectionNameConvention(t => true);

        public CamelCaseCollectionNameConvention(Func<Type, bool> matcher)
            : base(matcher)
        { }

        public string GetCollectionName(Type type)
        {
            return Inflector.ToCamelCase(type.Name);
        }
    }
}