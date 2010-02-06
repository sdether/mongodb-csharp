using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Configuration;
using MongoDB.Framework.Configuration.Fluent.Mapping;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Mapping.Visitors;

using NUnit.Framework;
using MongoDB.Framework.Proxy.Castle;

namespace MongoDB.Framework.Configuration.Mapping
{
    [TestFixture]
    public class MappingTests
    {
        [Test]
        public void Should_map_from_document_to_entity()
        {
            var fluentMapModelRegistry = new FluentMapModelRegistry()
                .AddMapsFromAssemblyContaining<PartyMap>();
            var configuration = new MongoConfiguration("tests", fluentMapModelRegistry);

            Guid id = Guid.NewGuid();
            var document = new Document()
                .Append("_id", new Binary(id.ToByteArray()) { Subtype = Binary.TypeCode.Uuid })
                .Append("Name", "Bob McBob")
                .Append("PhoneNumber", new Document()
                    .Append("AreaCode", "123")
                    .Append("Prefix", "456")
                    .Append("Number", "7890"))
                .Append("AlternatePhoneNumbers", new Document()
                    .Append("Home", new Document()
                        .Append("AreaCode", "111")
                        .Append("Prefix", "222")
                        .Append("Number", "3333"))
                    .Append("Work", new Document()
                        .Append("AreaCode", "444")
                        .Append("Prefix", "555")
                        .Append("Number", "6666")))
                .Append("Aliases", new[] { "Grumpy", "Dopey", "Sleepy" })
                .Append("Type", "Person")
                .Append("BirthDate", new DateTime(1900, 1, 1))
                .Append("not-mapped", true);

            var mongoSession = (IMongoSessionImplementor)configuration.CreateMongoSessionFactory().OpenMongoSession();
            var classMap = mongoSession.MappingStore.GetClassMapFor<Person>();
            var person = (Person)new DocumentToEntityMapper(mongoSession)
                .CreateEntity(classMap, document);
            Assert.IsNotNull(person);
            Assert.AreEqual(id, person.Id);
            Assert.AreEqual("Bob McBob", person.Name);
            Assert.AreEqual("123", person.PhoneNumber.AreaCode);
            Assert.AreEqual("456", person.PhoneNumber.Prefix);
            Assert.AreEqual("7890", person.PhoneNumber.Number);
            Assert.AreEqual(2, person.AlternatePhoneNumbers.Count);
            Assert.AreEqual(3, person.Aliases.Count);
            Assert.AreEqual(new DateTime(1900, 1, 1), person.BirthDate);
            Assert.AreEqual(1, person.ExtendedProperties.Count);
            Assert.AreEqual(true, person.ExtendedProperties["not-mapped"]);
        }

        [Test]
        public void Should_map_from_entity_to_document()
        {
            var fluentMapModelRegistry = new FluentMapModelRegistry()
                .AddMapsFromAssemblyContaining<PartyMap>();
            var configuration = new MongoConfiguration("tests", fluentMapModelRegistry);

            var person = new Person()
            {
                Id = Guid.NewGuid(),
                Name = "Bob McBob",
                BirthDate = new DateTime(1900, 1, 1),
                PhoneNumber = new PhoneNumber()
                {
                    AreaCode = "123",
                    Prefix = "456",
                    Number = "7890"
                },
                AlternatePhoneNumbers = new Dictionary<string, PhoneNumber>
                {
                    { "Home", new PhoneNumber() { AreaCode = "111", Prefix = "222", Number = "3333" } },
                    { "Work", new PhoneNumber() { AreaCode = "444", Prefix = "555", Number = "6666" } },
                },
                ExtendedProperties = new Dictionary<string, object>
                {
                    { "not-mapped", true }
                }
            };
            person.Aliases.Add("Grumpy");
            person.Aliases.Add("Dopey");
            person.Aliases.Add("Sleepy");

            var mongoSession = (IMongoSessionImplementor)configuration.CreateMongoSessionFactory().OpenMongoSession();
            var classMap = mongoSession.MappingStore.GetClassMapFor<Person>();
            var document = new EntityToDocumentMapper(mongoSession)
                .CreateDocument(person);

            Assert.AreEqual(person.Id.ToByteArray(), ((Binary)document["_id"]).Bytes);
            Assert.AreEqual("Bob McBob", document["Name"]);
            Assert.AreEqual("Person", document["Type"]);
            Assert.AreEqual(new DateTime(1900, 1, 1), document["BirthDate"]);
            Assert.AreEqual("123", ((Document)document["PhoneNumber"])["AreaCode"]);
            Assert.AreEqual("456", ((Document)document["PhoneNumber"])["Prefix"]);
            Assert.AreEqual("7890", ((Document)document["PhoneNumber"])["Number"]);
            Assert.AreEqual(new[] { "Grumpy", "Dopey", "Sleepy" }, document["Aliases"]);
            Assert.AreEqual(true, document["not-mapped"]);
        }



        public class PartyMap : FluentRootClass<Party>
        {
            public PartyMap()
            {
                CollectionName = "parties";
                DiscrimatorKey = "Type";

                HasIndex().Ascending("Name");

                Id(x => x.Id);

                Map(x => x.Name);
                Map(x => x.PhoneNumber);

                Collection(x => x.AlternatePhoneNumbers);
                Collection(x => x.Aliases);


                ExtendedProperties(x => x.ExtendedProperties);
            }
        }

        public class PersonMap : FluentSubClass<Person>
        {
            public PersonMap()
            {
                Discriminator = "Person";
                Map(x => x.BirthDate);
            }
        }

        public class OrganizationMap : FluentSubClass<Organization>
        {
            public OrganizationMap()
            {
                Discriminator = "Organization";
                Map(x => x.EmployeeCount);
            }
        }

        public class PhoneNumberMap : FluentNestedClass<PhoneNumber>
        {
            public PhoneNumberMap()
            {
                Map(x => x.AreaCode);
                Map(x => x.Prefix);
                Map(x => x.Number);
            }
        }

        public enum PartyType
        {
            Organization,
            Person
        }

        public class PhoneNumber
        {
            public string AreaCode { get; set; }
            public string Prefix { get; set; }
            public string Number { get; set; }
        }

        public abstract class Party
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public PhoneNumber PhoneNumber { get; set; }

            public IDictionary<string, PhoneNumber> AlternatePhoneNumbers { get; set; }

            public IList<string> Aliases { get; private set; }

            public abstract PartyType Type { get; }

            public IDictionary<string, object> ExtendedProperties { get; set; }

            public Party()
            {
                this.Aliases = new List<string>();
                this.AlternatePhoneNumbers = new Dictionary<string, PhoneNumber>();
                this.ExtendedProperties = new Dictionary<string, object>();
            }
        }

        public class Organization : Party
        {
            public int EmployeeCount { get; set; }

            public override PartyType Type
            {
                get { return PartyType.Organization; }
            }
        }

        public class Person : Party
        {
            public DateTime BirthDate { get; set; }

            public override PartyType Type
            {
                get { return PartyType.Person; }
            }
        }

    }
}