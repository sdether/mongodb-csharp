using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Linq.Visitors;
using MongoDB.Framework.Configuration.Mapping.Models;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Fluent
{
    public class FluentSubClass<TSubClass> : FluentClass<SubClassMapModel, TSubClass>
    {
        public FluentSubClass()
            : base(new SubClassMapModel(typeof(TSubClass)))
        { }
    }
}