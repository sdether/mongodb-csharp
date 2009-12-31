using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.Fluent;
using NUnit.Framework;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping
{
    [TestFixture]
    public class ManyToOneReferenceFixture : TestCase
    {
        protected override IMapProvider MapProvider
        {
            get 
            { 
                return new FluentMapProvider()
                    .AddMap(new EntityAMap())
                    .AddMap(new EntityBMap()); 
            }
        }

        protected override void BeforeTest()
        {
            using (var context = CreateContext())
            {
                var bId = Guid.NewGuid().ToString();
                context.Database.GetCollection("EntityB")
                    .Insert(new Document().Append("_id", bId).Append("Name", "Bob"));
                context.Database.GetCollection("EntityA")
                    .Insert(new Document().Append("_id", Guid.NewGuid().ToString()).Append("Name", "Jack").Append("B", new DBRef("EntityB", bId)));
            }
        }

        protected override void AfterTest()
        {
            using (var context = CreateContext())
            {
                context.Database.MetaData.DropCollection("EntityA");
                context.Database.MetaData.DropCollection("EntityB");
            }
        }

        [Test]
        public void Should_fetch_reference_automatically()
        {
            using (var context = CreateContext())
            {
                var a = context.FindOne<EntityA>(null);
                Assert.IsNotNull(a.B);
            }
        }

        [Test]
        public void Should_delete_referenced_entity()
        {
            using (var context = CreateContext())
            {
                var a = context.FindOne<EntityA>(null);
                context.Delete(a);
            }

            using (var context = CreateContext())
            {
                var b = context.FindOne<EntityB>(null);
                Assert.IsNull(b);
            }
        }
    }

    public class EntityA
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public EntityB B { get; set; }
    }

    public class EntityB
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }

    public class EntityAMap : FluentRootClassMap<EntityA>
    {
        public EntityAMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            References(x => x.B)
                .Cascade(Cascade.Delete);
        }
    }

    public class EntityBMap : FluentRootClassMap<EntityB>
    {
        public EntityBMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}
