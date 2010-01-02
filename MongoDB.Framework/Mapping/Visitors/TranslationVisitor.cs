﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Visitors
{
    public abstract class TranslationVisitor : NullMapVisitor
    {
        public override void ProcessClass(ClassMap classMap)
        {
            if (classMap.HasId)
                this.Visit(classMap.IdMap);

            foreach (var memberMap in classMap.MemberMaps)
                this.Visit(memberMap);

            if (classMap.HasExtendedProperties)
                this.Visit(classMap.ExtendedPropertiesMap);

            if (classMap.HasIndexes)
            {
                foreach (var index in classMap.Indexes)
                    this.Visit(index);
            }
        }

        public override void Visit(RootClassMap rootClassMap)
        {
            rootClassMap.Accept(this);
        }

        public override void Visit(NestedClassMap nestedClassMap)
        {
            nestedClassMap.Accept(this);
        }

        public override void Visit(SubClassMap subClassMap)
        {
            subClassMap.Accept(this);
        }

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