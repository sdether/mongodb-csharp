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
    public class NestedClass : TestCase
    {
        protected override IMapProvider MapProvider
        {
            get
            {
                return new FluentMapProvider()
                    .AddMap(new EntityMap())
                    .AddMap(new SubEntityMap());
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
            entity.SubEntity = new SubEntity()
            {
                Integer = 42,
                Double = 123.456
            };
            using (var context = this.CreateContext())
            {
                context.InsertOnSubmit(entity);
                context.SubmitChanges();
            }

            Document insertedDocument;
            using (var context = this.CreateContext())
            {
                insertedDocument = context.Database.GetCollection("Entity").FindOne(null);
            }

            Assert.IsNotNull(insertedDocument);
            Assert.AreEqual(entity.Id, new Guid((string)insertedDocument["_id"]));
            Assert.AreEqual(42, ((Document)insertedDocument["SubEntity"])["Integer"]);
            Assert.AreEqual(123.456, ((Document)insertedDocument["SubEntity"])["Double"]);
        }

        public class Entity
        {
            public Guid Id { get; private set; }

            public SubEntity SubEntity { get; set; }
        }

        public class SubEntity
        {
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
                Map(x => x.Double);
                Map(x => x.Integer);
            }
        }
    }
}