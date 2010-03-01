using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Configuration.Mapping
{
    public class ClassMapModel : ClassMapModelBase
    {
        public string CollectionName { get; set; }

        public string DiscriminatorKey { get; set; }

        public ExtendedPropertiesMapModel ExtendedPropertiesMap { get; set; }

        public IdMapModel IdMap { get; set; }

        public List<IndexModel> Indexes { get; private set; }

        public List<SubClassMapModel> SubClassMaps { get; private set; }

        public ClassMapModel(Type type)
            : base(type)
        {
            this.Indexes = new List<IndexModel>();
            this.SubClassMaps = new List<SubClassMapModel>();
        }
    }
}