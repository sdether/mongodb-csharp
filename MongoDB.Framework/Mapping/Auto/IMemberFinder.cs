using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Mapping.Auto
{
    public interface IMemberFinder
    {
        IEnumerable<MemberInfo> FindMembers(Type type);
    }
}