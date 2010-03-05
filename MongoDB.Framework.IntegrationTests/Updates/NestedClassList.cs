using System;
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
    public class NestedClassList : TestCase
    {
        protected override void BeforeTest()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                mongoSession.Database.GetCollection("Entity")
                    .Insert(new Document()
                        .Append("_id", Guid.NewGuid())
                        .Append("NestedEntities", new Document[] {
                            new Document()
                                .Append("Integer", 1)
                                .Append("Double", 1.1),
                            new Document()
                                .Append("Integer", 2)
                                .Append("Double", 2.2),
                            new Document()
                                .Append("Integer", 3)
                                .Append("Double", 3.3)}));
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
            using (var mongoSession = this.OpenMongoSession())
            {
                var entity = mongoSession.FindOne<Entity>(null);
                entity.NestedEntities.RemoveAt(1);
                entity.NestedEntities.RemoveAt(1);
                entity.NestedEntities.Add(new NestedEntity() { Double = 4.4, Integer = 4 });
                mongoSession.SubmitChanges();
            }

            Document insertedDocument;
            using (var mongoSession = this.OpenMongoSession())
            {
                insertedDocument = mongoSession.Database.GetCollection("Entity").FindOne(null);
            }

            Assert.IsNotNull(insertedDocument);
            var subEntities = (Document[])insertedDocument["NestedEntities"];
            Assert.AreEqual(2, subEntities.Length);
            Assert.AreEqual(1, subEntities[0]["Integer"]);
            Assert.AreEqual(1.1, subEntities[0]["Double"]);
            Assert.AreEqual(4, subEntities[1]["Integer"]);
            Assert.AreEqual(4.4, subEntities[1]["Double"]);
        }

        public class Entity
        {
            public Guid Id { get; private set; }

            public List<NestedEntity> NestedEntities { get; set; }
        }

        public class NestedEntity
        {
            public int Integer { get; set; }

            public double Double { get; set; }
        }
    }
}