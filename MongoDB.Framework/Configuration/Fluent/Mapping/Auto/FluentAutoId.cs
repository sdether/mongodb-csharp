using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Configuration.Mapping.Conventions;
using System.Reflection;

namespace MongoDB.Framework.Configuration.Fluent.Mapping.Auto
{
    public class FluentAutoId
    {
        private AutoMapModel autoMapModel;

        public FluentAutoId(AutoMapModel autoMapModel)
        {
            this.autoMapModel = autoMapModel;
        }

        public void IsNamed(string name)
        {
            this.autoMapModel.IdConvention = new NamedIdConvention(t => true, name);
        }

        public void IsNamed(string name, MemberTypes memberTypes, BindingFlags bindingFlags)
        {
            this.autoMapModel.IdConvention = new NamedIdConvention(t => true, name, memberTypes, bindingFlags);
        }
    }
}