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

        private static Dictionary<Type, IValueConverter> valueConverters = new Dictionary<Type, IValueConverter>()
        {
            { typeof(Guid), new GuidValueConverter() }
        };

        public static void RegisterValueConverter<T>(IValueConverter valueConverter)
        {
            RegisterValueConverter(typeof(T), valueConverter);
        }

        public static void RegisterValueConverter(Type type, IValueConverter valueConverter)
        {
            valueConverters[type] = valueConverter;
        }

        private DefaultValueConverterConvention()
            : base(m => true)
        { }

        public virtual IValueConverter GetValueConverter(MemberInfo memberInfo)
        {
            var type = ReflectionUtil.GetMemberValueType(memberInfo);
            IValueConverter valueConverter;
            if(valueConverters.TryGetValue(type, out valueConverter))
                return valueConverter;

            return new NullSafeValueConverter(type);
        }
    }
}