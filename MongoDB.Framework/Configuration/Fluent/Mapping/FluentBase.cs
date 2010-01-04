﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Configuration.Mapping.Models;
using System.Reflection;
using MongoDB.Framework.Linq.Visitors;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public abstract class FluentBase<TModel> where TModel : MapModel
    {
        public TModel Model { get; private set; }

        public FluentBase(TModel model)
        {
            this.Model = model;
        }

    }
}