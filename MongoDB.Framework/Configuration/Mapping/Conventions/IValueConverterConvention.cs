using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public interface IValueConverterConvention : IConvention<MemberInfo>
    {
        IValueConverter GetValueConverter(MemberInfo memberInfo);
    }
}
