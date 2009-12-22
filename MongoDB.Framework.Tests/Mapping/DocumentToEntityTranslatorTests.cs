using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Mapping.Fluent;

using NUnit.Framework;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework.Mapping
{
    [TestFixture]
    public class DocumentToEntityTranslatorTests
    {
        [Test]
        public void Should_hydrate_entity()
        {
            var mappingStore = new FluentMappingStore()
                .AddMapsFromAssemblyContaining<PartyMap>();

            var document = new Document()
                .Append("_id", new Oid("4b27b9f1cf24000000002aa0"))
                .Append("Name", "Bob McBob")
                .Append("PhoneNumber", new Document()
                    .Append("AreaCode", "123")
                    .Append("Prefix", "456")
                    .Append("Number", "7890"))
                .Append("Type", "Person")
                .Append("BirthDate", new DateTime(1900, 1, 1))
                .Append("not-mapped", true);

            var translator = new DocumentToEntityTranslator(mappingStore);

            var person = translator.Translate(typeof(Person), document) as Person;
            Assert.IsNotNull(person);
            Assert.AreEqual("Bob McBob", person.Name);
            Assert.AreEqual("123", person.PhoneNumber.AreaCode);
            Assert.AreEqual("456", person.PhoneNumber.Prefix);
            Assert.AreEqual("7890", person.PhoneNumber.Number);
            Assert.AreEqual(new DateTime(1900, 1, 1), person.BirthDate);
            Assert.AreEqual(1, person.ExtendedProperties.Count);
            Assert.AreEqual(true, person.ExtendedProperties["not-mapped"]);
        }
    }
}