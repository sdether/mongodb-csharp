using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration
{
    public abstract class MapVisitor : IMapVisitor
    {
        public virtual void VisitRootEntityMap(RootEntityMap rootEntityMap)
        { }

        public virtual void VisitEntityMap(EntityMap entityMap)
        { }

        public virtual void VisitDiscriminatedEntityMap(DiscriminatedEntityMap discriminatedEntityMap)
        { }

        public virtual void VisitPrimitiveMemberMap(PrimitiveMemberMap primitiveMemberMap)
        { }

        public virtual void VisitComponentMemberMap(ComponentMemberMap componentMemberMap)
        { }

        public virtual void VisitIdMap(IdMap idMap)
        { }

        public virtual void VisitIndex(Index index)
        { }
    }
}