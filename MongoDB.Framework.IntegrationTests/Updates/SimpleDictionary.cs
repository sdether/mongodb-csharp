using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Configuration.Fluent.Mapping;
using NUnit.Framework;
using MongoDB.Driver;

namespace MongoDB.Framework.Updates
{
    [TestFixture]
    public class SimpleDictionary : TestCase
    {
        protected override IMapProvider MapProvider
        {
            get
            {
                return new FluentMapProvider()
                    .AddMap(new EntityMap());
            }
        }

        protected override void BeforeTest()
        {
            using (var context = this.CreateContext())
            {
                context.Database.GetCollection("Entity")
                    .Insert(new Document()
                        .Append("_id", Guid.NewGuid().ToString())
                        .Append("Integers", new Document()
                            .Append("one", 1)
                            .Append("two", 2)
                            .Append("three", 3)));
            }
        }

        protected override void AfterTest()
        {
            using (var context = this.CreateContext())
            {
                context.Database.MetaData.DropCollection("Entity");
            }
        }

        [Test]
        public void Should_update()
        {
            using (var context = this.CreateContext())
            {
                var entity = context.FindOne<Entity>(null);
                entity.Integers.Clear();
                entity.Integers["four"] = 4;

                context.Update(entity);
            }

            Document insertedDocument;
            using (var context = this.CreateContext())
            {
                insertedDocument = context.Database.GetCollection("Entity").FindOne(null);
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