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

        protected override void BeforeTest()
        {
            using (var context = this.CreateContext())
            {
                context.Database.GetCollection("Entity")
                    .Insert(new Document()
                        .Append("_id", Guid.NewGuid().ToString())
                        .Append("SubEntity", new Document()
                            .Append("Integer", 42)
                            .Append("Double", 123.456)));
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
                entity.SubEntity.Integer = 43;
                entity.SubEntity.Double = 654.321;
                context.Update(entity);
            }

            Document updatedDocument;
            using (var context = this.CreateContext())
            {
                updatedDocument = context.Database.GetCollection("Entity").FindOne(null);
            }

            Assert.IsNotNull(updatedDocument);
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