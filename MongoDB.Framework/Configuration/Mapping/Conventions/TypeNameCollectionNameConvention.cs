using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public class TypeNameCollectionNameConvention : ConventionBase<Type>, ICollectionNameConvention
    {
        public static readonly TypeNameCollectionNameConvention AlwaysMatching = new TypeNameCollectionNameConvention(t => true);

        public TypeNameCollectionNameConvention(Func<Type, bool> matcher)
            : base(matcher)
        { }

        public string GetCollectionName(Type type)
        {
            return type.Name;
        }
    }
}