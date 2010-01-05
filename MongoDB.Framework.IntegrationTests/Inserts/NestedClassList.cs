using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration.Fluent.Mapping;
using MongoDB.Framework.Mapping;

using NUnit.Framework;

namespace MongoDB.Framework.Inserts
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
            entity.SubEntities = new List<SubEntity>()
            {
                { new SubEntity() { Integer = 1, Double = 1.1 } },
                { new SubEntity() { Integer = 2, Double = 2.2 } },
                { new SubEntity() { Integer = 3, Double = 3.3 } }
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
            var subEntities = (Document[])insertedDocument["SubEntities"];
            Assert.AreEqual(3, subEntities.Length);
            Assert.AreEqual(1, subEntities[0]["Integer"]);
            Assert.AreEqual(1.1, subEntities[0]["Double"]);
            Assert.AreEqual(2, subEntities[1]["Integer"]);
            Assert.AreEqual(2.2, subEntities[1]["Double"]);
            Assert.AreEqual(3, subEntities[2]["Integer"]);
            Assert.AreEqual(3.3, subEntities[2]["Double"]);
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