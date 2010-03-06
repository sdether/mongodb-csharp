using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Mapper.Configuration.Mapping;

namespace MongoDB.Mapper.Configuration.Fluent.Mapping
{
    public class FluentReference : FluentBase<ReferenceMapModel>
    {
        public FluentReference()
            : base(new ReferenceMapModel())
        { }

        public FluentReference Key(string key)
        {
            this.Model.Key = key;
            return this;
        }

        public FluentReference NotLazy()
        {
            this.Model.IsLazy = false;
            return this;
        }
    }
}