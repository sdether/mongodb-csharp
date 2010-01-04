using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Configuration.Mapping.Models;
using MongoDB.Framework.Configuration.Mapping.Types;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Fluent
{
    public class FluentEmbeddedValuePart
    {
        private EmbeddedValuePart embeddedValueMapModel;

        public FluentEmbeddedValuePart(EmbeddedValuePart embeddedValueMapModel)
        {
            this.embeddedValueMapModel = embeddedValueMapModel;
        }

        public FluentEmbeddedValuePart CustomTypeIs<TValueType>() where TValueType : IValueType, new()
        {
            return this.CustomTypeIs(new TValueType());
        }

        public FluentEmbeddedValuePart CustomTypeIs(IValueType valueType)
        {
            this.embeddedValueMapModel.CustomValueType = valueType;
            return this;
        }
    }
}