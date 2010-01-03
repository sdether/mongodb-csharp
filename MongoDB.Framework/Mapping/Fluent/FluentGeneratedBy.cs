using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.IdGenerators;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentGeneratedBy
    {
        private FluentId idMap;

        public FluentGeneratedBy(FluentId idMap)
        {
            this.idMap = idMap;
        }

        public FluentId Assigned()
        {
            this.idMap.Model.Generator = new AssignedGenerator();
            return this.idMap;
        }

        public FluentId Guid()
        {
            this.idMap.Model.Generator = new GuidGenerator();
            return this.idMap;
        }

        public FluentId GuidComb()
        {
            this.idMap.Model.Generator = new GuidCombGenerator();
            return this.idMap;
        }

        public FluentId Oid()
        {
            this.idMap.Model.Generator = new OidGenerator();
            return this.idMap;
        }
    }
}