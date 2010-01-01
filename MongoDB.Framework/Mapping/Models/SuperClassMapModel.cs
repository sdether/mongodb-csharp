using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Models
{
    public abstract class SuperClassMapModel : ClassMapModel
    {
        public string DiscriminatorKey { get; set; }

        public ExtendedPropertiesMapModel ExtendedPropertiesMap { get; set; }

        public List<SubClassMapModel> SubClassMaps { get; private set; }

        public SuperClassMapModel(Type type)
            : base(type)
        {
            this.SubClassMaps = new List<SubClassMapModel>();
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IMapModelVisitor visitor)
        {
            visitor.ProcessSuperClass(this);

            foreach (var subClassMap in this.SubClassMaps)
                visitor.Visit(subClassMap);

            visitor.Visit(this.ExtendedPropertiesMap);

            base.Accept(visitor);
        }
    }
}
