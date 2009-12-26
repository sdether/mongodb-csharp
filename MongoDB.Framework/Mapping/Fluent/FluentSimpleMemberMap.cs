using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentSimpleMemberMap : FluentMemberMap<SimpleMemberMap>
    {
        private readonly SimpleMemberMap instance;

        public FluentSimpleMemberMap()
        {
            this.instance = new SimpleMemberMap();
        }

        public override SimpleMemberMap Instance
        {
            get { return this.instance; }
        }
    }
}