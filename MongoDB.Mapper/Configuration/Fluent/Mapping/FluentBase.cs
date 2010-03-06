using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Mapper.Configuration.Mapping;
using System.Reflection;
using MongoDB.Mapper.Linq.Visitors;

namespace MongoDB.Mapper.Configuration.Fluent.Mapping
{
    public abstract class FluentBase<TModel> where TModel : ModelNode
    {
        public TModel Model { get; private set; }

        public FluentBase(TModel model)
        {
            this.Model = model;
        }

    }
}