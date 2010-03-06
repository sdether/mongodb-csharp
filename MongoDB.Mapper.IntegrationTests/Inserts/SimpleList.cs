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
    public class SimpleList : TestCase
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
            entity.Strings = new List<string>() { { "one" }, { "two" }, { "three" } };
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
            Assert.AreEqual(new [] { "one", "two", "three" }, insertedDocument["Strings"]);
        }

        public class Entity
        {
            public Guid Id { get; private set; }

            public List<string> Strings { get; set; }
        }
    }
}