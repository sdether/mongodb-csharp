using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace MongoDB.Framework.Mapping.Fluent
{
    [TestFixture]
    public class FluentCollectionTests
    {
        [Test]
        public void test()
        {
            var map = new PartyMap().Instance;
        }
    }

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
}