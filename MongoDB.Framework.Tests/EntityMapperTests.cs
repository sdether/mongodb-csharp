using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;

using NUnit.Framework;

namespace MongoDB.Framework
{
    [TestFixture]
    public class EntityMapperTests
    {

        [Test]
        public void Should_map_from_document_to_entity()
        {
            MongoConfiguration config = new MongoConfiguration();
            config.AddRootEntityMap(new PartyMap().Instance);
            var mapper = new EntityMapper(config);

            var document = new Document()
                .Append("_id", new Oid("4b27b9f1cf24000000002aa0"))
                .Append("Name", "Bob McBob")
                .Append("PhoneNumber", new Document()
                    .Append("area", "123")
                    .Append("pfx", "456")
                    .Append("num", "7890"))
                .Append("Type", "Person")
                .Append("BirthDate", new DateTime(1900, 1, 1))
                .Append("not-mapped", true);

            var entity = mapper.MapDocumentToEntity(document, typeof(Party));

            Assert.IsInstanceOfType(typeof(Person), entity);
            var person = (Person)entity;
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
            MongoConfiguration config = new MongoConfiguration();
            config.AddRootEntityMap(new PartyMap().Instance);
            var mapper = new EntityMapper(config);

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
                }
            };

            person.ExtendedProperties = new Dictionary<string, object>
            {
                { "not-mapped", true }
            };

            var document = mapper.MapEntityToDocument(person);

            Assert.AreEqual(new Oid("4b27b9f1cf24000000002aa0"), document["_id"]);
            Assert.AreEqual("Bob McBob", document["Name"]);
            Assert.AreEqual("Person", document["Type"]);
            Assert.AreEqual(new DateTime(1900, 1, 1), document["BirthDate"]);
            Assert.AreEqual("123", ((Document)document["PhoneNumber"])["area"]);
            Assert.AreEqual("456", ((Document)document["PhoneNumber"])["pfx"]);
            Assert.AreEqual("7890", ((Document)document["PhoneNumber"])["num"]);
            Assert.AreEqual(true, document["not-mapped"]);
        }

    }
}