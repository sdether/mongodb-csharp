using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Mapper.Mapping.Conventions
{
    public interface IMemberKeyConvention
    {
        string GetMemberKey(MemberInfo member);
    }
}