using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Mapping.Fluent;

namespace MongoDB.Framework
{
    public class PartyMap : FluentRootClassMap<Party>
    {
        public PartyMap()
        {
            UseCollection("parties");

            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.PhoneNumber);

            HasMany(x => x.Aliases);

            DiscriminateSubClassesOnKey<string>("ValueType")
                .SubClass<Person>(PartyType.Person.ToString(), m =>
                {
                    m.Map(x => x.BirthDate);
                })
                .SubClass<Organization>(PartyType.Organization.ToString(), m =>
                {
                    m.Map(x => x.EmployeeCount);
                });

            ExtendedProperties(x => x.ExtendedProperties);
        }
    }

    public class PhoneNumberMap : FluentNestedClassMap<PhoneNumber>
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
        public string Id { get; set; }

        public string Name { get; set; }

        public PhoneNumber PhoneNumber { get; set; }

        public IList<string> Aliases { get; private set; }

        public abstract PartyType Type { get; }

        public IDictionary<string, object> ExtendedProperties { get; set; }

        public Party()
        {
            this.Aliases = new List<string>();
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