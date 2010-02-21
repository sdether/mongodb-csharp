using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public class DefaultCollectionNameConvention : ConventionBase<Type>, ICollectionNameConvention
    {
        public static readonly DefaultCollectionNameConvention AlwaysMatching = new DefaultCollectionNameConvention(t => true);

        public DefaultCollectionNameConvention(Func<Type, bool> matcher)
            : base(matcher)
        { }

        public string GetCollectionName(Type type)
        {
            return type.Name;
        }
    }
}