using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using MongoDB.Framework.Mapping.Fluent;
using MongoDB.Driver;
using MongoDB.Framework.Configuration;

namespace MongoDB.Framework.Mapping
{
    [TestFixture]
    public class MappingTests
    {
        [Test]
        public void Should_map_from_document_to_entity()
        {
            var fluentMapProvider = new FluentMapProvider()
                .AddMapsFromAssemblyContaining<PartyMap>();
            var mappingStore = new MappingStore(fluentMapProvider);

            var configuration = new MongoConfiguration("tests", mappingStore);

            var document = new Document()
                .Append("_id", new Oid("4b27b9f1cf24000000002aa0"))
                .Append("Name", "Bob McBob")
                .Append("PhoneNumber", new Document()
                    .Append("AreaCode", "123")
                    .Append("Prefix", "456")
                    .Append("Number", "7890"))
                .Append("ValueType", "Person")
                .Append("BirthDate", new DateTime(1900, 1, 1))
                .Append("not-mapped", true);

            var mongoContext = configuration.CreateContextFactory().CreateContext();
            var classMap = mappingStore.GetClassMapFor<Person>();
            var mappingContext = new MappingContext(mongoContext, document, typeof(Person));
            classMap.MapFromDocument(mappingContext);
            var person = mappingContext.Entity as Person;
            Assert.IsNotNull(person);
            Assert.AreEqual("Bob McBob", person.Name);
            Assert.AreEqual("123", person.PhoneNumber.AreaCode);
            Assert.AreEqual("456", person.PhoneNumber.Prefix);
            Assert.AreEqual("7890", person.PhoneNumber.Number);
            Assert.AreEqual(new DateTime(1900, 1, 1), person.BirthDate);
            Assert.AreEqual(1, person.ExtendedProperties.Count);
            Assert.AreEqual(true, person.ExtendedProperties["not-mapped"]);
        }

        [Test]
        public void Should_map_from_entity_to_document()
        {
            var fluentMapProvider = new FluentMapProvider()
                .AddMapsFromAssemblyContaining<PartyMap>();
            var mappingStore = new MappingStore(fluentMapProvider);

            var person = new Person()
            {
                Id = "4b27b9f1cf24000000002aa0",
                Name = "Bob McBob",
                BirthDate = new DateTime(1900, 1, 1),
                PhoneNumber = new PhoneNumber()
                {
                    AreaCode = "123",
                    Prefix = "456",
                    Number = "7890"
                },
                ExtendedProperties = new Dictionary<string, object>
                {
                    { "not-mapped", true }
                }
            };

            var document = new Document();
            var classMap = mappingStore.GetClassMapFor<Person>();
            classMap.MapToDocument(person, document);

            Assert.AreEqual(new Oid("4b27b9f1cf24000000002aa0"), document["_id"]);
            Assert.AreEqual("Bob McBob", document["Name"]);
            Assert.AreEqual("Person", document["ValueType"]);
            Assert.AreEqual(new DateTime(1900, 1, 1), document["BirthDate"]);
            Assert.AreEqual("123", ((Document)document["PhoneNumber"])["AreaCode"]);
            Assert.AreEqual("456", ((Document)document["PhoneNumber"])["Prefix"]);
            Assert.AreEqual("7890", ((Document)document["PhoneNumber"])["Number"]);
            Assert.AreEqual(true, document["not-mapped"]);
        }

    }
}