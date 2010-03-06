using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Mapper.Linq.Visitors;
using MongoDB.Mapper.Configuration.Mapping;
using MongoDB.Mapper.Reflection;

namespace MongoDB.Mapper.Configuration.Fluent.Mapping
{
    public class FluentSubClass<TSubClass> : FluentClassBase<SubClassMapModel, TSubClass>
    {
        public FluentSubClass()
            : base(new SubClassMapModel(typeof(TSubClass)))
        { }
    }
}