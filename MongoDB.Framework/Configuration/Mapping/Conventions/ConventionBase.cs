using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public abstract class ConventionBase<T> : IConvention<T>
    {
        private Func<T, bool> matcher;

        public ConventionBase(Func<T, bool> matcher)
        {
            if (matcher == null)
                throw new ArgumentNullException("matcher");

            this.matcher = matcher;
        }

        public bool Matches(T member)
        {
            return matcher(member);
        }
    }
}
