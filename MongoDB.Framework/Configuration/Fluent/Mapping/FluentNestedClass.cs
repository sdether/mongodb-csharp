using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Framework.Configuration.Mapping;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public class FluentNestedClass<TNestedClass> : FluentSuperClass<NestedClassMapModel, TNestedClass>
    {
        public FluentNestedClass()
            : base(new NestedClassMapModel(typeof(TNestedClass)))
        { }
    }
}