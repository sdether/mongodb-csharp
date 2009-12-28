using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.Model;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentNestedClassMemberMapModel : FluentMemberMap<NestedClassMemberMapModel>
    {
        public FluentNestedClassMemberMapModel()
            : base(new NestedClassMemberMapModel())
        { }

        public FluentNestedClassMemberMapModel Key(string key)
        {
            this.Model.Key = key;
            return this;
        }
    }
}