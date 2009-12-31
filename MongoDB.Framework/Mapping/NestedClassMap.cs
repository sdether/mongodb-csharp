using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping
{
    public class NestedClassMap : SuperClassMap
    {
        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <value>The name of the collection.</value>
        public override string CollectionName
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the id map.
        /// </summary>
        /// <value>The id map.</value>
        public override IdMap IdMap
        {
            get { return null; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NestedClassMap"/> class.
        /// </summary>
        /// <param name="type">ValueType of the entity.</param>
        public NestedClassMap(Type type, IEnumerable<MemberMap> memberMaps, string discriminatorKey, object discriminator, IEnumerable<SubClassMap> subClassMaps, ExtendedPropertiesMap extendedPropertiesMap)
            : base(type, memberMaps, discriminatorKey, discriminator, subClassMaps, extendedPropertiesMap)
        { }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IMapVisitor visitor)
        {
            visitor.ProcessNestedClass(this);

            base.Accept(visitor);
        }
    }
}