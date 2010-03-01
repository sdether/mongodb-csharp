using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Configuration.Mapping
{
    public abstract class ClassMapModelBase : ModelNode
    {
        /// <summary>
        /// Gets or sets the class activator.
        /// </summary>
        /// <value>The class activator.</value>
        public IClassActivator ClassActivator { get; set; }

        /// <summary>
        /// Gets or sets the discriminator.
        /// </summary>
        /// <value>The discriminator.</value>
        public object Discriminator { get; set; }

        /// <summary>
        /// Gets or sets the persistent member maps.
        /// </summary>
        /// <value>The persistent member maps.</value>
        public List<PersistentMemberMapModel> PersistentMemberMaps { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassMapModel"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public ClassMapModelBase(Type type)
        {
            this.PersistentMemberMaps = new List<PersistentMemberMapModel>();
            this.Type = type;
        }
    }
}