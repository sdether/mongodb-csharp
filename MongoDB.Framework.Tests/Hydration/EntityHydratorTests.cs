using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Mapping.Fluent;

using NUnit.Framework;
using MongoDB.Framework.Tracking;

namespace MongoDB.Framework.Hydration
{
    [TestFixture]
    public class EntityHydratorTests
    {
        [Test]
        public void Should_hydrate_entity()
        {
            FluentMappingStore mappingStore = new FluentMappingStore();
            mappingStore.AddMapsFromAssemblyContaining<PartyMap>();

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

            var hydrator = new EntityHydrator(mappingStore, new DefaultChangeTracker(mappingStore));

            var person = hydrator.HydrateEntity<Person>(document);
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