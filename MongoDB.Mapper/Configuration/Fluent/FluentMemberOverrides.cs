using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Mapper.Mapping.Auto;

namespace MongoDB.Mapper.Configuration.Fluent
{
    public class FluentMemberOverrides
    {
        private MemberOverrides memberOverrides;

        public FluentMemberOverrides(MemberOverrides memberOverrides)
        {
            this.memberOverrides = memberOverrides;
        }

        public FluentMemberOverrides KeyIs(string name)
        {
            this.memberOverrides.Key = name;
            return this;
        }

        public FluentMemberOverrides IsReference()
        {
            this.memberOverrides.IsReference = true;
            return this;
        }

        public FluentMemberOverrides Exclude()
        {
            this.memberOverrides.Exclude = true;
            return this;
        }
    }
}