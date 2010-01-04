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
    public class NestedClassList : TestCase
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
                        .Append("SubEntities", new Document[] {
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
                entity.SubEntities.RemoveAt(1);
                entity.SubEntities.RemoveAt(1);
                entity.SubEntities.Add(new SubEntity() { Double = 4.4, Integer = 4 });
                context.Update(entity);
            }

            Document insertedDocument;
            using (var context = this.CreateContext())
            {
                insertedDocument = context.Database.GetCollection("Entity").FindOne(null);
            }

            Assert.IsNotNull(insertedDocument);
            var subEntities = (Document[])insertedDocument["SubEntities"];
            Assert.AreEqual(2, subEntities.Length);
            Assert.AreEqual(1, subEntities[0]["Integer"]);
            Assert.AreEqual(1.1, subEntities[0]["Double"]);
            Assert.AreEqual(4, subEntities[1]["Integer"]);
            Assert.AreEqual(4.4, subEntities[1]["Double"]);
        }

        public class Entity
        {
            public Guid Id { get; private set; }

            public List<SubEntity> SubEntities { get; set; }
        }

        public class SubEntity
        {
            public int Integer { get; set; }

            public double Double { get; set; }
        }

        public class EntityMap : FluentRootClass<Entity>
        {
            public EntityMap()
            {
                Id(x => x.Id);
                Collection(x => x.SubEntities);
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