using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Configuration.Mapping.Conventions;


namespace MongoDB.Framework.Configuration.Fluent.Mapping.Conventions
{
    public class FluentAutoId
    {
        private MappingConventions conventions;

        public FluentAutoId(MappingConventions conventions)
        {
            this.conventions = conventions;
        }

        public void IsNamed(string name)
        {
            this.conventions.IdConvention = new NamedIdConvention(t => true, name);
        }

        public void IsNamed(string name, MemberTypes memberTypes, BindingFlags bindingFlags)
        {
            this.conventions.IdConvention = new NamedIdConvention(t => true, name, memberTypes, bindingFlags);
        }
    }
}