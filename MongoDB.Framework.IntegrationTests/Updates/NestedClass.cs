﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration.Fluent.Mapping;
using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Mapping;

using NUnit.Framework;

namespace MongoDB.Framework.Updates
{
    [TestFixture]
    public class NestedClass : TestCase
    {
        protected override void BeforeTest()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                mongoSession.Database.GetCollection("Entity")
                    .Insert(new Document()
                        .Append("_id", new Binary(Guid.NewGuid().ToByteArray()) { Subtype = Binary.TypeCode.Uuid })
                        .Append("NestedEntity", new Document()
                            .Append("_id", new Binary(Guid.NewGuid().ToByteArray()) { Subtype = Binary.TypeCode.Uuid })
                            .Append("Integer", 42)
                            .Append("Double", 123.456)));
            }
        }

        protected override void AfterTest()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                mongoSession.Database.MetaData.DropCollection("Entity");
            }
        }

        [Test]
        public void Should_update()
        {
            Guid subEntityId;
            using (var mongoSession = this.OpenMongoSession())
            {
                var entity = mongoSession.FindOne<Entity>(null);
                subEntityId = entity.NestedEntity.Id;
                entity.NestedEntity.Integer = 43;
                entity.NestedEntity.Double = 654.321;
                mongoSession.SubmitChanges();
            }

            Document updatedDocument;
            using (var mongoSession = this.OpenMongoSession())
            {
                updatedDocument = mongoSession.Database.GetCollection("Entity").FindOne(null);
            }

            Assert.IsNotNull(updatedDocument);
            Assert.AreEqual(subEntityId, new Guid(((Binary)((Document)updatedDocument["NestedEntity"])["_id"]).Bytes));
            Assert.AreEqual(43, ((Document)updatedDocument["NestedEntity"])["Integer"]);
            Assert.AreEqual(654.321, ((Document)updatedDocument["NestedEntity"])["Double"]);
        }

        public class Entity
        {
            public Guid Id { get; private set; }

            public NestedEntity NestedEntity { get; set; }
        }

        public class NestedEntity
        {
            public Guid Id { get; private set; }
            public double Double { get; set; }

            public int Integer { get; set; }
        }
    }
}