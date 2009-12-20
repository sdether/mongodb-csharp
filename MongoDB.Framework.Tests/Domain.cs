using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Configuration.Fluent;
using MongoDB.Framework.Configuration;
using MongoDB.Driver;

namespace MongoDB.Framework
{
    public class PartyMap : FluentRootEntityMap<Party>
    {
        public PartyMap()
        {
            WithCollectionName("party");
            Index("phone_area").Ascending("PhoneNumber.area");

            Id(x => x.Id);
            Map("Name");

            Component<PhoneNumber>(x => x.PhoneNumber, m =>
            {
                m.Map(x => x.AreaCode, "area");
                m.Map(x => x.Prefix, "pfx");
                m.Map(x => x.Number, "num");
            });

            DiscriminateBy<string>("Type")
                .Entity<Organization>(PartyType.Organization.ToString(), m =>
                {
                    m.Map(x => x.EmployeeCount);
                })
                .Entity<Person>(PartyType.Person.ToString(), m =>
                {
                    m.Map(x => x.BirthDate);
                });

            UseExtendedProperties(x => x.ExtendedProperties);
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
        public string Id { get; set; }

        public string Name { get; set; }

        public PhoneNumber PhoneNumber { get; set; }

        public abstract PartyType Type { get; }

        public IDictionary<string, object> ExtendedProperties { get; set; }

        public Party()
        {
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