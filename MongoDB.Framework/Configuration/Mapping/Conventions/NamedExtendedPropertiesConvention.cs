using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public class NamedExtendedPropertiesConvention : ConventionBase<Type>, IExtendedPropertiesConvention
    {
        private string name;
        private BindingFlags bindingFlags;
        private MemberTypes memberTypes;

        public NamedExtendedPropertiesConvention(Func<Type, bool> matcher, string name)
            : this(matcher, name, MemberTypes.Property | MemberTypes.Field, BindingFlags.Instance | BindingFlags.Public)
        { }

        public NamedExtendedPropertiesConvention(Func<Type, bool> matcher, string name, MemberTypes memberTypes, BindingFlags bindingFlags)
            : base(matcher)
        {
            this.name = name;
            this.memberTypes = memberTypes;
            this.bindingFlags = bindingFlags;
        }

        public ExtendedPropertiesMapModel  GetExtendedPropertiesMapModel(Type type)
        {
            MemberInfo memberInfo = type.GetMember(name, this.memberTypes, this.bindingFlags).Single();
            if (memberInfo == null)
                throw new NotSupportedException();

            return new ExtendedPropertiesMapModel()
            {
                Getter = memberInfo,
                Setter = memberInfo
            };
        }

        public bool HasExtendedProperties(Type type)
        {
            return type.GetMember(name, this.memberTypes, this.bindingFlags).SingleOrDefault() != null;
        }
    }
}