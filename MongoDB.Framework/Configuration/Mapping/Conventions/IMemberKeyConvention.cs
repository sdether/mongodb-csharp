using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public interface IMemberKeyConvention : IConvention<MemberInfo>
    {
        string GetKey(MemberInfo member);
    }
}