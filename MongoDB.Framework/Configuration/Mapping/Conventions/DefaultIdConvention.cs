using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public class DefaultIdConvention : ConventionBase<Type>, IIdConvention
    {
        public static readonly DefaultIdConvention AlwaysMatching = new DefaultIdConvention();

        private DefaultIdConvention()
            : base(t => true)
        { }

        public IdMapModel GetIdMapModel(Type type)
        {
            throw new NotSupportedException();
        }

        public bool HasId(Type type)
        {
            return false;
        }
    }
}