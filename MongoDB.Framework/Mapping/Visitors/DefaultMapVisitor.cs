﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Visitors
{
    public abstract class DefaultMapVisitor : NullMapVisitor
    {
        public override void Visit(ClassMap classMap)
        {
            classMap.Accept(this);
        }

        public override void Visit(IdMap idMap)
        {
            idMap.Accept(this);
        }

        public override void Visit(MemberMap memberMap)
        {
            memberMap.Accept(this);
        }

        public override void Visit(ManyToOneMap manyToOneMap)
        {
            manyToOneMap.Accept(this);
        }

        public override void Visit(ExtendedPropertiesMap extendedPropertiesMap)
        {
            extendedPropertiesMap.Accept(this);
        }

        public override void Visit(Index indexMap)
        {
            indexMap.Accept(this);
        }
    }
}