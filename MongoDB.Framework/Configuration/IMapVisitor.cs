using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration
{
    public interface IMapVisitor
    {
        void VisitRootEntityMap(RootEntityMap rootEntityMap);

        void VisitEntityMap(EntityMap entityMap);

        void VisitDiscriminatedEntityMap(DiscriminatedEntityMap discriminatedEntityMap);

        void VisitMemberMap(MemberMap memberMap);

        void VisitComponentMap(ComponentMap componentMap);

        void VisitIdMap(IdMap idMap);
    }
}