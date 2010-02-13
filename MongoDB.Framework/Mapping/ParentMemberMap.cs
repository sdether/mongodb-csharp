using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class ParentMemberMap : MemberMap
    {
        public ParentMemberMap(string memberName, Func<object, object> memberGetter, Action<object, object> memberSetter)
            : base(memberName, memberGetter, memberSetter)
        { }

        public override void Accept(IMapVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
