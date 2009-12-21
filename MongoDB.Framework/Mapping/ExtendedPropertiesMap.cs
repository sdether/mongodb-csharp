﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public sealed class ExtendedPropertiesMap : Map
    {
        public string MemberName { get; private set; }

        public Type MemberType { get; private set; }

        public Func<object, object> MemberGetter { get; private set; }

        public Action<object, object> MemberSetter { get; private set; }

        public ExtendedPropertiesMap(string memberName, Type memberType, Func<object, object> memberGetter, Action<object, object> memberSetter)
        {
            if (memberName == null)
                throw new ArgumentException("Cannot be null or empty.", "memberName");
            if (memberType == null)
                throw new ArgumentNullException("memberType");
            if (memberGetter == null)
                throw new ArgumentNullException("memberGetter");
            if (memberSetter == null)
                throw new ArgumentNullException("memberSetter");

            this.MemberName = memberName;
            this.MemberType = memberType;
            this.MemberGetter = memberGetter;
            this.MemberSetter = memberSetter;
        }
    }
}