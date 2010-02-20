using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public class NamedIdConvention : ConventionBase<Type>, IIdConvention
    {
        private string name;
        private BindingFlags bindingFlags;
        private MemberTypes memberTypes;

        public NamedIdConvention(Func<Type, bool> matcher, string name)
            : this(matcher, name, MemberTypes.Property | MemberTypes.Field, BindingFlags.Instance | BindingFlags.Public)
        { }

        public NamedIdConvention(Func<Type, bool> matcher, string name, MemberTypes memberTypes, BindingFlags bindingFlags)
            : base(matcher)
        {
            this.name = name;
            this.memberTypes = memberTypes;
            this.bindingFlags = bindingFlags;
        }

        public IdMapModel GetIdMapModel(Type type)
        {
            MemberInfo memberInfo = type.GetMember(name, this.memberTypes, this.bindingFlags).Single();
            if (memberInfo == null)
                throw new NotSupportedException();

            return new IdMapModel()
            {
                Getter = memberInfo,
                Setter = memberInfo
            };
        }

        public bool HasId(Type type)
        {
            return type.GetMember(name, this.memberTypes, this.bindingFlags).SingleOrDefault() != null;
        }
    }
}