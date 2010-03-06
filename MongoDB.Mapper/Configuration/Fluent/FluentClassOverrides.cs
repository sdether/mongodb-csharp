using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Mapper.Mapping.Auto;

namespace MongoDB.Mapper.Configuration.Fluent
{
    public class FluentClassOverrides
    {
        private ClassOverrides classOverrides;

        public FluentClassOverrides(ClassOverrides classOverrides)
        {
            this.classOverrides = classOverrides;
        }

        public FluentClassOverrides CollectionNameIs(string name)
        {
            this.classOverrides.CollectionName = name;
            return this;
        }
    }
}