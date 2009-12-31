using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Framework.Mapping.Models;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentNestedClassMap<TNestedClass> : FluentSuperClassMap<NestedClassMapModel, TNestedClass>
    {
        public FluentNestedClassMap()
            : base(new NestedClassMapModel(typeof(TNestedClass)))
        { }
    }
}