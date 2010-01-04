using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Configuration.Fluent.Mapping;
using NUnit.Framework;
using MongoDB.Driver;

namespace MongoDB.Framework.Inserts
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

        protected override void AfterTest()
        {
            using (var context = this.CreateContext())
            {
                context.Database.MetaData.DropCollection("Entity");
            }
        }

        [Test]
        public void Should_insert()
        {
            var entity = new Entity();
            entity.Integers = new Dictionary<string, int>() { { "one", 1 }, { "two", 2}, { "three", 3 } };
            using (var context = this.CreateContext())
            {
                context.Insert(entity);
            }

            Document insertedDocument;
            using (var context = this.CreateContext())
            {
                insertedDocument = context.Database.GetCollection("Entity").FindOne(null);
            }

            Assert.IsNotNull(insertedDocument);
            Assert.AreEqual(entity.Id, new Guid((string)insertedDocument["_id"]));
            Assert.AreEqual(1, ((Document)insertedDocument["Integers"])["one"]);
            Assert.AreEqual(2, ((Document)insertedDocument["Integers"])["two"]);
            Assert.AreEqual(3, ((Document)insertedDocument["Integers"])["three"]);
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