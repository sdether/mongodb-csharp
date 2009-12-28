using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Model
{
    public abstract class SuperClassMapModel : ClassMapModel
    {
        public string DiscriminatorKey { get; set; }

        public MemberMapModel ExtendedPropertiesMap { get; set; }

        public List<SubClassMapModel> SubClassMaps { get; private set; }

        public SuperClassMapModel(Type type)
            : base(type)
        {
            this.SubClassMaps = new List<SubClassMapModel>();
        }
    }
}
