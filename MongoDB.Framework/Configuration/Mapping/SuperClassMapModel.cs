using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping
{
    public abstract class SuperClassMapModel : ClassMapModel
    {
        public string DiscriminatorKey { get; set; }

        public ExtendedPropertiesMapModel ExtendedPropertiesMap { get; set; }

        public IdMapModel IdMap { get; set; }

        public List<SubClassMapModel> SubClassMaps { get; private set; }

        public Type ClassActivatorType { get; set; }

        public SuperClassMapModel(Type type)
            : base(type)
        {
            this.SubClassMaps = new List<SubClassMapModel>();
        }
    }
}
