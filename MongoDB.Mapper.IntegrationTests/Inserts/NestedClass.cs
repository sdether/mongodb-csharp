using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Mapper.Configuration.Fluent.Mapping;
using MongoDB.Mapper.Configuration.Mapping;
using MongoDB.Mapper.Mapping;

using NUnit.Framework;

namespace MongoDB.Mapper.Inserts
{
    [TestFixture]
    public class NestedClass : TestCase
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
            entity.NestedEntity = new NestedEntity()
            {
                Integer = 42,
                Double = 123.456
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
            Assert.AreEqual(entity.NestedEntity.Id, ((Document)insertedDocument["NestedEntity"])["_id"]);
            Assert.AreEqual(42, ((Document)insertedDocument["NestedEntity"])["Integer"]);
            Assert.AreEqual(123.456, ((Document)insertedDocument["NestedEntity"])["Double"]);
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