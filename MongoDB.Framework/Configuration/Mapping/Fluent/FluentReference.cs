using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Configuration.Mapping.Models;

namespace MongoDB.Framework.Configuration.Mapping.Fluent
{
    public class FluentReference : FluentBase<ManyToOneMapModel>
    {
        public FluentReference()
            : base(new ManyToOneMapModel())
        { }

        public FluentReference Key(string key)
        {
            this.Model.Key = key;
            return this;
        }
    }
}