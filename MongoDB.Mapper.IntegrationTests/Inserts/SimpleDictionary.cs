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
    public class SimpleDictionary : TestCase
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
            entity.Integers = new Dictionary<string, int>() { { "one", 1 }, { "two", 2}, { "three", 3 } };
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
            Assert.AreEqual(1, ((Document)insertedDocument["Integers"])["one"]);
            Assert.AreEqual(2, ((Document)insertedDocument["Integers"])["two"]);
            Assert.AreEqual(3, ((Document)insertedDocument["Integers"])["three"]);
        }

        public class Entity
        {
            public Guid Id { get; private set; }

            public Dictionary<string,int> Integers { get; set; }
        }
    }
}