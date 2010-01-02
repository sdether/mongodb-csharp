﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping.Visitors
{
    public class DeleteDocumentMapper : TranslationVisitor
    {
        private Document document;
        private object entity;
        private IMongoContext mongoContext;

        public DeleteDocumentMapper(IMongoContext mongoContext)
        {
            if (mongoContext == null)
                throw new ArgumentNullException("mongoContext");

            this.mongoContext = mongoContext;
        }

        public Document CreateDocument(ClassMap classMap, object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            this.entity = entity;

            this.document = new Document();
            classMap.Accept(this);

            return this.document;
        }

        public override void ProcessId(IdMap idMap)
        {
            var value = idMap.MemberGetter(this.entity);
            value = idMap.ValueType.ConvertToDocumentValue(value, this.mongoContext);
            this.document[idMap.Key] = value;
        }


    }
}