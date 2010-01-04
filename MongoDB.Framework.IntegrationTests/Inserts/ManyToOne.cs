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
    public class ManyToOne : TestCase
    {
        protected override IMapProvider MapProvider
        {
            get 
            {
                return new FluentMapProvider()
                    .AddMap(new EntityMap())
                    .AddMap(new EntityRefMap());
            }
        }

        protected override void AfterTest()
        {
            using (var context = this.CreateContext())
            {
                context.Database.MetaData.DropCollection("Entity");
                context.Database.MetaData.DropCollection("EntityRef");
            }
        }

        [Test]
        public void Should_insert()
        {
            var reference = new EntityRef();
            var entity = new Entity();
            entity.Reference = reference;
            using (var context = this.CreateContext())
            {
                context.Insert(reference);
                context.Insert(entity);
            }

            Document insertedDocument;
            using (var context = this.CreateContext())
            {
                insertedDocument = context.Database.GetCollection("Entity").FindOne(null);
            }

            Assert.IsNotNull(insertedDocument);
            Assert.AreEqual(entity.Id, new Guid((string)insertedDocument["_id"]));
            Assert.AreEqual(reference.Id, new Guid((string)((DBRef)insertedDocument["Reference"]).Id));
        }

        public class Entity
        {
            public Guid Id { get; private set; }

            public EntityRef Reference { get; set; }
        }

        public class EntityRef
        {
            public Guid Id { get; private set; }
        }

        public class EntityMap : FluentRootClass<Entity>
        {
            public EntityMap()
            {
                Id(x => x.Id);
                References(x => x.Reference);
            }
        }

        public class EntityRefMap : FluentRootClass<EntityRef>
        {
            public EntityRefMap()
            {
                Id(x => x.Id);
            }
        }

    }
}