using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public class FluentSubClass<TSubClass> : FluentClassBase<SubClassMapModel, TSubClass>
    {
        public FluentSubClass()
            : base(new SubClassMapModel(typeof(TSubClass)))
        { }
    }
}