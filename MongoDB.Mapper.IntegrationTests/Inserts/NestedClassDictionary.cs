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
    public class NestedClassDictionary : TestCase
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
            entity.NestedEntities = new Dictionary<string, NestedEntity>()
            {
                { "one", new NestedEntity() { Integer = 1, Double = 1.1 } },
                { "two", new NestedEntity() { Integer = 2, Double = 2.2 } },
                { "three", new NestedEntity() { Integer = 3, Double = 3.3 } }
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
            var subEntities = (Document)insertedDocument["NestedEntities"];
            Assert.AreEqual(3, subEntities.Count);
            Assert.AreEqual(1, ((Document)subEntities["one"])["Integer"]);
            Assert.AreEqual(1.1, ((Document)subEntities["one"])["Double"]);
            Assert.AreEqual(2, ((Document)subEntities["two"])["Integer"]);
            Assert.AreEqual(2.2, ((Document)subEntities["two"])["Double"]);
            Assert.AreEqual(3, ((Document)subEntities["three"])["Integer"]);
            Assert.AreEqual(3.3, ((Document)subEntities["three"])["Double"]);
        }

        public class Entity
        {
            public Guid Id { get; private set; }

            public Dictionary<string, NestedEntity> NestedEntities { get; set; }
        }

        public class NestedEntity
        {
            public int Integer { get; set; }

            public double Double { get; set; }
        }
    }
}