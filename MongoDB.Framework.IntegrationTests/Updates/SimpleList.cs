using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Mapping;
using MongoDB.Framework.Mapping.Fluent;
using NUnit.Framework;
using MongoDB.Driver;

namespace MongoDB.Framework.Updates
{
    [TestFixture]
    public class SimpleList : TestCase
    {
        protected override IMapProvider MapProvider
        {
            get
            {
                return new FluentMapProvider()
                    .AddMap(new EntityMap());
            }
        }

        protected override void BeforeTest()
        {
            using (var context = this.CreateContext())
            {
                context.Database.GetCollection("Entity")
                    .Insert(new Document()
                        .Append("_id", Guid.NewGuid().ToString())
                        .Append("Strings", new [] { "one", "two", "three" }));
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
                entity.Strings.Remove("two");
                entity.Strings.Remove("three");
                entity.Strings.Add("four");
                context.Update(entity);
            }

            Document updatedDocument;
            using (var context = this.CreateContext())
            {
                updatedDocument = context.Database.GetCollection("Entity").FindOne(null);
            }

            Assert.IsNotNull(updatedDocument);
            Assert.AreEqual(new [] { "one", "four" }, updatedDocument["Strings"]);
        }

        public class Entity
        {
            public Guid Id { get; private set; }

            public List<string> Strings { get; set; }
        }

        public class EntityMap : FluentRootClassMap<Entity>
        {
            public EntityMap()
            {
                Id(x => x.Id);
                Map(x => x.Strings);
            }
        }
    }
}