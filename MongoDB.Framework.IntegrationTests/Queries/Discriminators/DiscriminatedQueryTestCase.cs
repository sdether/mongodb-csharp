using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Configuration.Fluent.Mapping;
using MongoDB.Driver;

namespace MongoDB.Framework.Queries
{
    public abstract class DiscriminatedQueryTestCase : TestCase
    {
        protected override MongoDB.Framework.Configuration.Mapping.IMapModelRegistry MapModelRegistry
        {
            get
            {
                return new FluentMapModelRegistry()
                    .AddMap(new PartyMap());
            }
        }

        protected override void BeforeTest()
        {
            using (var mongoSession = this.OpenMongoSession())
            {
                var party1 = new Document()
                    .Append("_id", Guid.NewGuid().ToString("N"))
                    .Append("Type", "Person")
                    .Append("Name", "Bob McBob")
                    .Append("BirthDate", new DateTime(1900, 1, 1));
                var party2 = new Document()
                    .Append("_id", Guid.NewGuid().ToString("N"))
                    .Append("Type", "Person")
                    .Append("Name", "Jane McJane")
                    .Append("BirthDate", new DateTime(2000, 2, 2));
                var party3 = new Document()
                    .Append("_id", Guid.NewGuid().ToString("N"))
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

        public class PartyMap : FluentRootClass<Party>
        {
            public PartyMap()
            {
                Id(x => x.Id);
                Map(x => x.Name);

                DiscriminateSubClassesOnKey<string>("Type")
                    .SubClass<Person>(PartyType.Person.ToString(), m =>
                    {
                        m.Map(x => x.BirthDate);
                    })
                    .SubClass<Organization>(PartyType.Organization.ToString(), m =>
                    {
                        m.Map(x => x.EmployeeCount);
                    });
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
