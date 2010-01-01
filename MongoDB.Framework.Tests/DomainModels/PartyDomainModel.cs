using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Mapping.Fluent;

namespace MongoDB.Framework.DomainModels
{
    public class PartyMap : FluentRootClassMap<Party>
    {
        public PartyMap()
        {
            UseCollection("parties");

            Id(x => x.Id);

            Map.One(x => x.Name);
            Map.One(x => x.PhoneNumber);

            Map.Many(x => x.AlternatePhoneNumbers);
            Map.Many(x => x.Aliases);

            DiscriminateSubClassesOnKey<string>("Type")
                .SubClass<Person>(PartyType.Person.ToString(), m =>
                {
                    m.Map.One(x => x.BirthDate);
                })
                .SubClass<Organization>(PartyType.Organization.ToString(), m =>
                {
                    m.Map.One(x => x.EmployeeCount);
                });

            ExtendedProperties(x => x.ExtendedProperties);
        }
    }

    public class PhoneNumberMap : FluentNestedClassMap<PhoneNumber>
    {
        public PhoneNumberMap()
        {
            Map.One(x => x.AreaCode);
            Map.One(x => x.Prefix);
            Map.One(x => x.Number);
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