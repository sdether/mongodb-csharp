using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Configuration.Mapping;
using System.Reflection;
using MongoDB.Framework.Linq.Visitors;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
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