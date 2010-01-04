using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Configuration.Mapping.Models;
using MongoDB.Framework.Configuration.Mapping.Types;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Configuration.Fluent.Mapping
{
    public class FluentValue : FluentMember<ValueMapModel, FluentValue>
    {
        protected override FluentValue Fluent
        {
            get { return this; }
        }

        public FluentValue()
            : base(new ValueMapModel())
        { }

        public FluentValue CustomTypeIs<TValueType>() where TValueType : IValueType, new()
        {
            return this.CustomTypeIs(new TValueType());
        }

        public FluentValue CustomTypeIs(IValueType valueType)
        {
            this.Model.CustomValueType = valueType;
            return this;
        }
    }
}