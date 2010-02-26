using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Configuration.Mapping
{
    public abstract class SuperClassMapModel : ClassMapModel
    {
        public string DiscriminatorKey { get; set; }

        public ExtendedPropertiesMapModel ExtendedPropertiesMap { get; set; }

        public IdMapModel IdMap { get; set; }

        public List<SubClassMapModel> SubClassMaps { get; private set; }

        public SuperClassMapModel(Type type)
            : base(type)
        {
            this.SubClassMaps = new List<SubClassMapModel>();
        }
    }
}
