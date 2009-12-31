using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public sealed class ExtendedPropertiesMap : Map
    {
        public string MemberName { get; private set; }

        public Func<object, object> MemberGetter { get; private set; }

        public Action<object, object> MemberSetter { get; private set; }

        public ExtendedPropertiesMap(string memberName, Func<object, object> memberGetter, Action<object, object> memberSetter)
        {
            if (memberName == null)
                throw new ArgumentException("Cannot be null or empty.", "memberName");
            if (memberGetter == null)
                throw new ArgumentNullException("memberGetter");
            if (memberSetter == null)
                throw new ArgumentNullException("memberSetter");

            this.MemberName = memberName;
            this.MemberGetter = memberGetter;
            this.MemberSetter = memberSetter;
        }

        public override void Accept(IMapVisitor visitor)
        {
            visitor.ProcessExtendedProperties(this);
        }
    }
}