using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Mapping.Fluent;

namespace MongoDB.Framework
{
    public class PartyMap : FluentCollectionMap<Party>
    {
        public PartyMap()
        {
            UseCollection("parties");

            Id(x => x.Id);
            Map(x => x.Name);

            Component(x => x.PhoneNumber, m =>
            {
                m.Map(x => x.AreaCode);
                m.Map(x => x.Prefix);
                m.Map(x => x.Number);
            });

            DiscriminatedBy<string>("Type", m =>
            {
                m.Sub<Person>(PartyType.Person.ToString(), sm =>
                {
                    sm.Map(x => x.BirthDate);
                });

                m.Sub<Organization>(PartyType.Organization.ToString(), sm =>
                {
                    sm.Map(x => x.EmployeeCount);
                });
            });

            ExtendedProperties(x => x.ExtendedProperties);
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