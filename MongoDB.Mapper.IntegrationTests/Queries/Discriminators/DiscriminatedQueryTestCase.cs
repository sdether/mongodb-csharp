using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Mapper.Configuration.Fluent.Mapping;
using MongoDB.Driver;
using MongoDB.Mapper.Mapping;

namespace MongoDB.Mapper.Queries
{
    public abstract class DiscriminatedQueryTestCase : TestCase
    {
        protected override IMappingStore MappingStore
        {
            get
            {
                return new FluentMapModelRegistry()
                    .AddMap(new PartyMap())
                    .AddMap(new PersonMap())
                    .AddMap(new OrganizationMap())
                    .BuildMappingStore();
            }
        }

        protected override void BeforeTest()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                var party1 = new Document()
                    .Append("_id", Guid.NewGuid())
                    .Append("Type", "Person")
                    .Append("Name", "Bob McBob")
                    .Append("BirthDate", new DateTime(1900, 1, 1));
                var party2 = new Document()
                    .Append("_id", Guid.NewGuid())
                    .Append("Type", "Person")
                    .Append("Name", "Jane McJane")
                    .Append("BirthDate", new DateTime(2000, 2, 2));
                var party3 = new Document()
                    .Append("_id", Guid.NewGuid())
                    .Append("Type", "Organization")
                    .Append("Name", "The Muffler Shop")
                    .Append("EmployeeCount", 23);

                mongoSession.Database.GetCollection("Party")
                    .Insert(new[] { party1, party2, party3 });
            }
        }

        protected override void AfterTest()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                mongoSession.Database.MetaData.DropCollection("Party");
            }
        }

        public class PartyMap : FluentClass<Party>
        {
            public PartyMap()
            {
                DiscrimatorKey = "Type";
                Id(x => x.Id);
                Map(x => x.Name);

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

        public enum PartyType
        {
            Organization,
            Person
        }

        public abstract class Party
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public abstract PartyType Type { get; }
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
