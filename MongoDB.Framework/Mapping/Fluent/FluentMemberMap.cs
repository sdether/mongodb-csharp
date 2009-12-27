using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.Types;
using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Reflection;
using System.Reflection;
using System.Linq.Expressions;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentMemberMap : FluentMap<MemberMap>
    {
        private MemberMap memberMap;

        public override MemberMap Instance
        {
            get { return this.memberMap; }
        }

        public FluentMemberMap()
        {
            this.memberMap = new MemberMap();
        }

        public FluentMemberMap Key(string key)
        {
            this.memberMap.Key = key;
            return this;
        }
    }
}