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

        protected override void BeforeTest()
        {
            using (var context = this.CreateContext())
            {
                var refId = Guid.NewGuid().ToString();
                context.Database.GetCollection("EntityRef")
                    .Insert(new Document().Append("_id", refId));
                context.Database.GetCollection("Entity")
                    .Insert(new Document()
                        .Append("_id", Guid.NewGuid().ToString())
                        .Append("Reference", new DBRef("EntityRef", refId)));
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
        public void Should_update()
        {
            using (var context = this.CreateContext())
            {
                var entity = context.FindOne<Entity>(null);
                entity.Reference = null;
                context.SubmitChanges();
            }

            Document updatedDocument;
            using (var context = this.CreateContext())
            {
                updatedDocument = context.Database.GetCollection("Entity").FindOne(null);
            }

            Assert.IsNotNull(updatedDocument);
            Assert.AreEqual(MongoDBNull.Value, updatedDocument["Reference"]);
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