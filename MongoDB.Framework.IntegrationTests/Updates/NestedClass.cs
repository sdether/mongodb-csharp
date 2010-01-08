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
    public class NestedClass : TestCase
    {
        protected override IMapModelRegistry MapModelRegistry
        {
            get
            {
                return new FluentMapModelRegistry()
                    .AddMap(new EntityMap())
                    .AddMap(new SubEntityMap());
            }
        }

        protected override void BeforeTest()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                mongoSession.Database.GetCollection("Entity")
                    .Insert(new Document()
                        .Append("_id", Guid.NewGuid().ToString("N"))
                        .Append("SubEntity", new Document()
                            .Append("_id", Guid.NewGuid().ToString("N"))
                            .Append("Integer", 42)
                            .Append("Double", 123.456)));
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
            Guid subEntityId;
            using (var mongoSession = this.OpenMongoSession())
            {
                var entity = mongoSession.FindOne<Entity>(null);
                subEntityId = entity.SubEntity.Id;
                entity.SubEntity.Integer = 43;
                entity.SubEntity.Double = 654.321;
                mongoSession.SubmitChanges();
            }

            Document updatedDocument;
            using (var mongoSession = this.OpenMongoSession())
            {
                updatedDocument = mongoSession.Database.GetCollection("Entity").FindOne(null);
            }

            Assert.IsNotNull(updatedDocument);
            Assert.AreEqual(subEntityId, new Guid((string)((Document)updatedDocument["SubEntity"])["_id"]));
            Assert.AreEqual(43, ((Document)updatedDocument["SubEntity"])["Integer"]);
            Assert.AreEqual(654.321, ((Document)updatedDocument["SubEntity"])["Double"]);
        }

        public class Entity
        {
            public Guid Id { get; private set; }

            public SubEntity SubEntity { get; set; }
        }

        public class SubEntity
        {
            public Guid Id { get; private set; }
            public double Double { get; set; }

            public int Integer { get; set; }
        }

        public class EntityMap : FluentRootClass<Entity>
        {
            public EntityMap()
            {
                Id(x => x.Id);
                Map(x => x.SubEntity);
            }
        }

        public class SubEntityMap : FluentNestedClass<SubEntity>
        {
            public SubEntityMap()
            {
                Id(x => x.Id);
                Map(x => x.Double);
                Map(x => x.Integer);
            }
        }
    }
}