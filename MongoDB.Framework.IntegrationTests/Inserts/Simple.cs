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
    public class Simple : TestCase
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
            entity.String = "S";
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
            Assert.AreEqual("S", insertedDocument["String"]);
        }

        public class Entity
        {
            public Guid Id { get; private set; }

            public string String { get; set; }
        }

        public class EntityMap : FluentRootClass<Entity>
        {
            public EntityMap()
            {
                Id(x => x.Id);
                Map(x => x.String);
            }
        }
    }
}