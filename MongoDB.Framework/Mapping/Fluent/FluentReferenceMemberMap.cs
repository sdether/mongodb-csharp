using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.Models;
using MongoDB.Framework.Mapping.Types;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentReferenceMemberMap : FluentMemberMap<ReferenceMemberMapModel>
    {

        public FluentReferenceMemberMap()
            : base(new ReferenceMemberMapModel())
        { }

        public FluentReferenceMemberMap Key(string key)
        {
            this.Model.Key = key;
            return this;
        }

    }
}