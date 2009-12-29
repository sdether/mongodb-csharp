using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.IdGenerators;

namespace MongoDB.Framework.Mapping.Fluent
{
    public class FluentGeneratedBy
    {
        private FluentIdMap idMap;

        public FluentGeneratedBy(FluentIdMap idMap)
        {
            this.idMap = idMap;
        }

        public FluentIdMap Assigned()
        {
            this.idMap.Model.Generator = new AssignedGenerator();
            return this.idMap;
        }

        public FluentIdMap Guid()
        {
            this.idMap.Model.Generator = new GuidGenerator();
            return this.idMap;
        }

        public FluentIdMap GuidComb()
        {
            this.idMap.Model.Generator = new GuidCombGenerator();
            return this.idMap;
        }

        public FluentIdMap Oid()
        {
            this.idMap.Model.Generator = new OidGenerator();
            return this.idMap;
        }
    }
}