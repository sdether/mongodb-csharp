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

            if (classMap.HasExtendedProperties)
                classMap.ExtendedPropertiesMap.Accept(this);

            if (classMap.HasIndexes)
            {
                foreach (var index in classMap.Indexes)
                    index.Accept(this);
            }
        }

        public override void Visit(MemberMap memberMap)
        {
            memberMap.ValueType.Accept(this);
        }
    }
}