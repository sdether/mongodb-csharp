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
    public class SimpleDictionary : TestCase
    {
        protected override IMapModelRegistry MapModelRegistry
        {
            get
            {
                return new FluentMapModelRegistry()
                    .AddMap(new EntityMap());
            }
        }

        protected override void BeforeTest()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                mongoSession.Database.GetCollection("Entity")
                    .Insert(new Document()
                        .Append("_id", Guid.NewGuid().ToString("N"))
                        .Append("Integers", new Document()
                            .Append("one", 1)
                            .Append("two", 2)
                            .Append("three", 3)));
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
                entity.Integers.Clear();
                entity.Integers["four"] = 4;

                mongoSession.SubmitChanges();
            }

            Document insertedDocument;
            using (var mongoSession = this.OpenMongoSession())
            {
                insertedDocument = mongoSession.Database.GetCollection("Entity").FindOne(null);
            }

            Assert.IsNotNull(insertedDocument);
            Assert.AreEqual(1, ((Document)insertedDocument["Integers"]).Count);
            Assert.AreEqual(4, ((Document)insertedDocument["Integers"])["four"]);
        }

        public class Entity
        {
            public Guid Id { get; private set; }

            public Dictionary<string,int> Integers { get; set; }
        }

        public class EntityMap : FluentRootClass<Entity>
        {
            public EntityMap()
            {
                Id(x => x.Id);
                Collection(x => x.Integers);
            }
        }
    }
}