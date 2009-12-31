using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.Models;

namespace MongoDB.Framework.Mapping.Fluent
{
    public abstract class FluentMap<TModel> where TModel : MapModel
    {
        public TModel Model { get; private set; }

        public FluentMap(TModel model)
        {
            this.Model = model;
        }
    }
}