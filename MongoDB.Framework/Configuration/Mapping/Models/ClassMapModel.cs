using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping.Models
{
    public abstract class ClassMapModel : MapModel
    {
        /// <summary>
        /// Gets or sets the discriminator.
        /// </summary>
        /// <value>The discriminator.</value>
        public object Discriminator { get; set; }

        /// <summary>
        /// Gets the many to one maps.
        /// </summary>
        /// <value>The many to one maps.</value>
        public List<ManyToOneMapModel> ManyToOneMaps { get; private set; }

        /// <summary>
        /// Gets the member maps.
        /// </summary>
        /// <value>The member maps.</value>
        public List<MemberMapModel> MemberMaps { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassMapModel"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public ClassMapModel(Type type)
        {
            this.ManyToOneMaps = new List<ManyToOneMapModel>();
            this.MemberMaps = new List<MemberMapModel>();
            this.Type = type;
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IMapModelVisitor visitor)
        {
            visitor.ProcessClass(this);

            foreach (var memberMap in this.MemberMaps)
                visitor.Visit(memberMap);

            foreach (var manyToOneMap in this.ManyToOneMaps)
                visitor.Visit(manyToOneMap);
        }
    }
}