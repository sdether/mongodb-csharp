using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using MongoDB.Driver;

namespace MongoDB.Framework.Configuration.Visitors
{
    [TestFixture]
    public class EntityToDocumentTranslatorTests
    {
        [Test]
        public void Should_translate_entity_to_document()
        {
            MongoConfiguration config = new MongoConfiguration();
            config.AddRootEntityMap(new PartyMap().Instance);

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

            var visitor = new EntityToDocumentTranslator(person);
            config.GetRootEntityMapFor<Person>().Accept(visitor);
            var document = visitor.Document;

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