using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration.Fluent.Mapping;
using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Mapping;

using NUnit.Framework;

namespace MongoDB.Framework.Inserts
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
                    .AddMap(new NestedEntityMap());
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
        public void Should_insert()
        {
            var entity = new Entity();
            entity.NestedEntity = new NestedEntity()
            {
                Integer = 42,
                Double = 123.456
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
            Assert.AreEqual(entity.NestedEntity.Id, ((Document)insertedDocument["NestedEntity"])["_id"]);
            Assert.AreEqual(42, ((Document)insertedDocument["NestedEntity"])["Integer"]);
            Assert.AreEqual(123.456, ((Document)insertedDocument["NestedEntity"])["Double"]);
        }

        public class Entity
        {
            public Guid Id { get; private set; }

            public NestedEntity NestedEntity { get; set; }
        }

        public class NestedEntity
        {
            public Guid Id { get; private set; }

            public double Double { get; set; }

            public int Integer { get; set; }
        }

        public class EntityMap : FluentClass<Entity>
        {
            public EntityMap()
            {
                Id(x => x.Id);
                Map(x => x.NestedEntity);
            }
        }

        public class NestedEntityMap : FluentClass<NestedEntity>
        {
            public NestedEntityMap()
            {
                Id(x => x.Id);
                Map(x => x.Double);
                Map(x => x.Integer);
            }
        }
    }
}