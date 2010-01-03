using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Framework.Mapping.Models;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentNestedClass<TNestedClass> : FluentSuperClass<NestedClassMapModel, TNestedClass>
    {
        public FluentNestedClass()
            : base(new NestedClassMapModel(typeof(TNestedClass)))
        { }
    }
}