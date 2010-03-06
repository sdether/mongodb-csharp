﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration.Fluent.Mapping;
using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Mapping;

using NUnit.Framework;

namespace MongoDB.Framework.Inserts
{
    [TestFixture]
    public class NestedClassList : TestCase
    {
        protected override void AfterTest()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                mongoSession.Database.MetaData.DropCollection("Entity");
            }
        }

        [Test]
        public void Should_insert()
        {
            var entity = new Entity();
            entity.SubEntities = new List<SubEntity>()
            {
                { new SubEntity() { Integer = 1, Double = 1.1 } },
                { new SubEntity() { Integer = 2, Double = 2.2 } },
                { new SubEntity() { Integer = 3, Double = 3.3 } }
            };

            using (var mongoSession = this.OpenMongoSession())
            {
                mongoSession.InsertOnSubmit(entity);
                mongoSession.SubmitChanges();
            }

            Document insertedDocument;
            using (var mongoSession = this.OpenMongoSession())
            {
                insertedDocument = mongoSession.Database.GetCollection("Entity").FindOne(null);
            }

            Assert.IsNotNull(insertedDocument);
            Assert.AreEqual(entity.Id, insertedDocument["_id"]);
            var subEntities = (Document[])insertedDocument["SubEntities"];
            Assert.AreEqual(3, subEntities.Length);
            Assert.AreEqual(1, subEntities[0]["Integer"]);
            Assert.AreEqual(1.1, subEntities[0]["Double"]);
            Assert.AreEqual(2, subEntities[1]["Integer"]);
            Assert.AreEqual(2.2, subEntities[1]["Double"]);
            Assert.AreEqual(3, subEntities[2]["Integer"]);
            Assert.AreEqual(3.3, subEntities[2]["Double"]);
        }

        public class Entity
        {
            public Guid Id { get; private set; }

            public List<SubEntity> SubEntities { get; set; }
        }

        public class SubEntity
        {
            public int Integer { get; set; }

            public double Double { get; set; }
        }
    }
}