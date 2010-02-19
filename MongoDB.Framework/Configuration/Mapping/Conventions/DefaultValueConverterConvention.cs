using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Mapping.ValueConverters;
using System.Text.RegularExpressions;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public class DefaultValueConverterConvention : ConventionBase<MemberInfo>, IValueConverterConvention
    {
        public static readonly DefaultValueConverterConvention AlwaysMatching = new DefaultValueConverterConvention();

        private DefaultValueConverterConvention()
            : base(m => true)
        { }

        public IValueConverter GetValueConverter(MemberInfo memberInfo)
        {
            var type = ReflectionUtil.GetMemberValueType(memberInfo);
            if (type == typeof(Guid))
                return new GuidValueConverter();

            if (type == typeof(Regex))
            {
                //TODO: create a regex value type
            }

            return new NullSafeValueConverter(type);
        }
    }
}