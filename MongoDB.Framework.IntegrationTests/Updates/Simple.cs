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
    public class Simple : TestCase
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
                        .Append("_id", new Binary(Guid.NewGuid().ToByteArray()) { Subtype = Binary.TypeCode.Uuid })
                        .Append("String", "s"));
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
                entity.String = "t";
                mongoSession.SubmitChanges();
            }

            Document updatedDocument;
            using (var mongoSession = this.OpenMongoSession())
            {
                updatedDocument = mongoSession.Database.GetCollection("Entity").FindOne(null);
            }

            Assert.IsNotNull(updatedDocument);
            Assert.AreEqual("t", updatedDocument["String"]);
        }

        public class Entity
        {
            public Guid Id { get; private set; }

            public string String { get; set; }
        }

        public class EntityMap : FluentClass<Entity>
        {
            public EntityMap()
            {
                Id(x => x.Id);
                Map(x => x.String);
            }
        }
    }
}