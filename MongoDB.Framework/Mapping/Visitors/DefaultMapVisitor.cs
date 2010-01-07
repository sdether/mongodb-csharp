using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Visitors
{
    public abstract class DefaultMapVisitor : NullMapVisitor
    {
        public override void Visit(ClassMap classMap)
        {
            if (classMap.HasId)
                classMap.IdMap.Accept(this);

            foreach (var memberMap in classMap.MemberMaps)
                memberMap.Accept(this);

            foreach (var manyToOneMap in classMap.ManyToOneMaps)
                manyToOneMap.Accept(this);

            if (classMap.HasExtendedProperties)
                classMap.ExtendedPropertiesMap.Accept(this);

            if (classMap.HasIndexes)
            {
                foreach (var index in classMap.Indexes)
                    index.Accept(this);
            }
        }
    }
}