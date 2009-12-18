using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using MongoDB.Driver;

namespace MongoDB.Framework.Configuration.Visitors
{
    [TestFixture]
    public class DocumentToEntityTranslatorTests
    {
        [Test]
        public void Should_translate_document_to_entity()
        {
            MongoConfiguration config = new MongoConfiguration();
            config.AddRootEntityMap(new PartyMap().Instance);

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

            var visitor = new DocumentToEntityTranslator(document);
            config.GetRootEntityMapFor<Person>().Accept(visitor);
            var entity = visitor.Entity;

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
    }
}