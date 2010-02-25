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
  public class EntityWithoutConstructor
  {
    public Guid Id { get; set; }

    public string Title { get; set; }
  }

  public class EntityWithoutConstructorMap : FluentRootClass<EntityWithoutConstructor>
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
    #region IClassActivator Members

    public ClassMap Map { get; set; }

    public object Activate(Type type, Document doc)
    {
      Binary bin = (Binary)doc["_id"];
      Guid id = new Guid(bin.Bytes);
      return Activator.CreateInstance(type, id, (string)doc["Title"]);
    }

    #endregion
  }

  public class EntityWithConstructorMap : FluentRootClass<EntityWithConstructor>
  {
    public EntityWithConstructorMap()
    {
      UseActivator<ActivatorForEntityWithConstructor>();
      Id(m => m.Id);
      Map(m => m.Title);
    }
  }

  [TestFixture]
  public class ActivationTests
  {
    [Test]
    public void Should_activate_with_default_activator()
    {
      var fluentMapModelRegistry = new FluentMapModelRegistry()
        .AddMapsFromAssemblyContaining<EntityWithoutConstructor>();
      var configuration = new MongoConfiguration("tests", fluentMapModelRegistry);

      Guid id = Guid.NewGuid();
      var document = new Document()
        .Append("_id", new Binary(id.ToByteArray()) { Subtype = Binary.TypeCode.Uuid })
        .Append("Title", "test title");

      var mongoSession = (IMongoSessionImplementor)configuration.CreateMongoSessionFactory().OpenMongoSession();
      var classMap = mongoSession.MappingStore.GetClassMapFor<EntityWithoutConstructor>();
      var entity = (EntityWithoutConstructor)new DocumentToEntityMapper(mongoSession)
          .CreateEntity(classMap, document);

      Assert.IsNotNull(entity);
      Assert.AreEqual(id, entity.Id);
      Assert.AreEqual("test title", entity.Title);
    }

    [Test]
    public void Should_activate_with_custom_activator()
    {
      var fluentMapModelRegistry = new FluentMapModelRegistry()
        .AddMapsFromAssemblyContaining<EntityWithConstructor>();
      var configuration = new MongoConfiguration("tests", fluentMapModelRegistry);

      Guid id = Guid.NewGuid();
      var document = new Document()
        .Append("_id", new Binary(id.ToByteArray()) { Subtype = Binary.TypeCode.Uuid })
        .Append("Title", "test title");

      var mongoSession = (IMongoSessionImplementor)configuration.CreateMongoSessionFactory().OpenMongoSession();
      var classMap = mongoSession.MappingStore.GetClassMapFor<EntityWithConstructor>();
      var entity = (EntityWithConstructor)new DocumentToEntityMapper(mongoSession)
          .CreateEntity(classMap, document);

      Assert.IsNotNull(entity);
      Assert.AreEqual(id, entity.Id);
      Assert.AreEqual("test title", entity.Title);
    }
  }
}
