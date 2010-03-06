using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using MongoDB.Framework.Configuration;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping.Auto
{
    [TestFixture]
    public class AutoMapperTests
    {
        [Test]
        public void MapToDocument()
        {
            var mappingStore = new AutoMappingStore(new AutoMapper());
            var mapper = mappingStore.CreateMongoMapper();

            var person = new Person()
            {
                Id = Guid.NewGuid(),
                Address = new Address()
                {
                    Street = "123 Main ST",
                    City = "Anyplace",
                    State = "TX",
                    Country = "USA"
                }
            };

            var document = mapper.MapToDocument(person);

            Assert.AreEqual(person.Id, document["_id"]);
            Assert.AreEqual(person.Address.Street, ((Document)document["Address"])["Street"]);
            Assert.AreEqual(person.Address.City, ((Document)document["Address"])["City"]);
            Assert.AreEqual(person.Address.State, ((Document)document["Address"])["State"]);
            Assert.AreEqual(person.Address.Country, ((Document)document["Address"])["Country"]);
        }

        [Test]
        public void MapFromDocument()
        {
            var mappingStore = new AutoMappingStore(new AutoMapper());
            var mapper = mappingStore.CreateMongoMapper();

            var id = Guid.NewGuid();
            var doc = new Document()
                .Append("_id", id)
                .Append("Address", new Document()
                    .Append("Street", "123 Main ST")
                    .Append("City", "Anyplace")
                    .Append("State", "TX")
                    .Append("Country", "USA"));

            var person = mapper.MapToEntity<Person>(doc);

            Assert.AreEqual(id, person.Id);
            Assert.AreEqual("123 Main ST", person.Address.Street);
            Assert.AreEqual("Anyplace", person.Address.City);
            Assert.AreEqual("TX", person.Address.State);
            Assert.AreEqual("USA", person.Address.Country);
        }

        private class Person
        {
            public Guid Id { get; set; }
            public Address Address { get; set; }
        }

        private class Address
        {
            public string Street;
            public string City;
            public string State;
            public string Country;
        }
    }
}