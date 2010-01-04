using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Framework.Configuration.Mapping.Fluent;
using NUnit.Framework;
using MongoDB.Driver;

namespace MongoDB.Framework.Inserts
{
    [TestFixture]
    public class NestedClassDictionary : TestCase
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
            entity.SubEntities = new Dictionary<string, SubEntity>()
            {
                { "one", new SubEntity() { Integer = 1, Double = 1.1 } },
                { "two", new SubEntity() { Integer = 2, Double = 2.2 } },
                { "three", new SubEntity() { Integer = 3, Double = 3.3 } }
            };

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
            var subEntities = (Document)insertedDocument["SubEntities"];
            Assert.AreEqual(3, subEntities.Count);
            Assert.AreEqual(1, ((Document)subEntities["one"])["Integer"]);
            Assert.AreEqual(1.1, ((Document)subEntities["one"])["Double"]);
            Assert.AreEqual(2, ((Document)subEntities["two"])["Integer"]);
            Assert.AreEqual(2.2, ((Document)subEntities["two"])["Double"]);
            Assert.AreEqual(3, ((Document)subEntities["three"])["Integer"]);
            Assert.AreEqual(3.3, ((Document)subEntities["three"])["Double"]);
        }

        public class Entity
        {
            public Guid Id { get; private set; }

            public Dictionary<string, SubEntity> SubEntities { get; set; }
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