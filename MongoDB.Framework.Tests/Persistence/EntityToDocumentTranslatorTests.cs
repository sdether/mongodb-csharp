using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Mapping.Fluent;

using NUnit.Framework;
using MongoDB.Driver;

namespace MongoDB.Framework.Persistence
{
    [TestFixture]
    public class EntityToDocumentTranslatorTests
    {
        [Test]
        public void Should_translate_entity_to_document()
        {
            FluentMappingStore mappingStore = new FluentMappingStore();
            mappingStore.GetMapsFromAssemblyContaining<PartyMap>();

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

            var translator = new EntityToDocumentTranslator(mappingStore);
            var document = translator.Translate(person); 

            Assert.AreEqual(new Oid("4b27b9f1cf24000000002aa0"), document["_id"]);
            Assert.AreEqual("Bob McBob", document["Name"]);
            Assert.AreEqual("Person", document["Type"]);
            Assert.AreEqual(new DateTime(1900, 1, 1), document["BirthDate"]);
            Assert.AreEqual("123", ((Document)document["PhoneNumber"])["AreaCode"]);
            Assert.AreEqual("456", ((Document)document["PhoneNumber"])["Prefix"]);
            Assert.AreEqual("7890", ((Document)document["PhoneNumber"])["Number"]);
            Assert.AreEqual(true, document["not-mapped"]);
        }
    }
}
