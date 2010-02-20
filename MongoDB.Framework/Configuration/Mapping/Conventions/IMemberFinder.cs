using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public interface IMemberFinder : IConvention<Type>
    {
        IEnumerable<MemberInfo> FindMembers(Type type);
    }
}