using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Models
{
    public class RootClassMapModel : SuperClassMapModel
    {
        public string CollectionName { get; set; }

        public IdMapModel IdMap { get; set; }

        public RootClassMapModel(Type type)
            : base(type)
        { }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IMapModelVisitor visitor)
        {
            visitor.ProcessRootClass(this);

            visitor.Visit(this.IdMap);

            base.Accept(visitor);
        }
    }
}
