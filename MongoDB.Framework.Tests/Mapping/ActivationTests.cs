using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MongoDB.Framework.Configuration.Fluent.Mapping;
using MongoDB.Framework.Configuration.Mapping;
using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Mapping.Visitors;

namespace MongoDB.Framework.Mapping
{
    [TestFixture]
    public class ActivationTests
    {
        [Test]
        public void Should_activate_with_default_activator()
        {
            var fluentMapModelRegistry = new FluentMapModelRegistry()
              .AddMapsFromAssemblyContaining<EntityWithoutConstructor>();
            var mapper = fluentMapModelRegistry.BuildMappingStore().CreateMongoMapper();

            Guid id = Guid.NewGuid();
            var document = new Document()
              .Append("_id", id)
              .Append("Title", "test title");

            var entity = mapper.MapToEntity<EntityWithoutConstructor>(document);

            Assert.IsNotNull(entity);
            Assert.AreEqual(id, entity.Id);
            Assert.AreEqual("test title", entity.Title);
        }

        [Test]
        public void Should_activate_with_custom_activator()
        {
            var fluentMapModelRegistry = new FluentMapModelRegistry()
              .AddMapsFromAssemblyContaining<EntityWithConstructor>();
            var mapper = fluentMapModelRegistry.BuildMappingStore().CreateMongoMapper();

            Guid id = Guid.NewGuid();
            var document = new Document()
              .Append("_id", id)
              .Append("Title", "test title");

            var entity = mapper.MapToEntity<EntityWithConstructor>(document);

            Assert.IsNotNull(entity);
            Assert.AreEqual(id, entity.Id);
            Assert.AreEqual("test title", entity.Title);
        }
    }

    public class EntityWithoutConstructor
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
    }

    public class EntityWithoutConstructorMap : FluentClass<EntityWithoutConstructor>
    {
        public EntityWithoutConstructorMap()
        {
            Id(m => m.Id);
            Map(m => m.Title);
        }
    }

    public class EntityWithConstructor
    {
        public EntityWithConstructor(Guid id, string title)
        {
            Id = id;
            Title = title;
        }

        public Guid Id { get; set; }

        public string Title { get; set; }
    }

    public class ActivatorForEntityWithConstructor : IClassActivator
    {
        public ClassMapBase Map { get; set; }

        public object Activate(Type type, Document doc)
        {
            return Activator.CreateInstance(type, doc["id"], (string)doc["Title"]);
        }
    }

    public class EntityWithConstructorMap : FluentClass<EntityWithConstructor>
    {
        public EntityWithConstructorMap()
        {
            ActivateWith<ActivatorForEntityWithConstructor>();

            Id(m => m.Id);
            Map(m => m.Title);
        }
    }
}