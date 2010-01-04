using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Framework.Configuration.Mapping.Models;

namespace MongoDB.Framework.Configuration.Mapping.Fluent
{
    public class FluentNestedClass<TNestedClass> : FluentSuperClass<NestedClassMapModel, TNestedClass>
    {
        public FluentNestedClass()
            : base(new NestedClassMapModel(typeof(TNestedClass)))
        { }
    }
}