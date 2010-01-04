using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping.Models
{
    public abstract class ClassMapModel : MapModel
    {
        /// <summary>
        /// Gets the collection maps.
        /// </summary>
        /// <value>The collections.</value>
        public List<CollectionMapModel> CollectionMaps { get; private set; }

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
        /// Gets the value maps.
        /// </summary>
        /// <value>The value maps.</value>
        public List<ValueMapModel> ValueMaps { get; private set; }

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
            this.CollectionMaps = new List<CollectionMapModel>();
            this.ManyToOneMaps = new List<ManyToOneMapModel>();
            this.ValueMaps = new List<ValueMapModel>();
            this.Type = type;
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IMapModelVisitor visitor)
        {
            visitor.ProcessClass(this);

            foreach (var valueMap in this.ValueMaps)
                visitor.Visit(valueMap);

            foreach (var collectionMap in this.CollectionMaps)
                visitor.Visit(collectionMap);

            foreach (var manyToOneMap in this.ManyToOneMaps)
                visitor.Visit(manyToOneMap);
        }
    }
}