using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace MongoDB.Mapper.Configuration
{
    [TestFixture]
    public class FluentConfigurationTests
    {
        [Test]
        public void Should_configure()
        {
            var sessionFactory = Fluently.Configure()
                .Database("tests")
                .Mappings(m => m.CreateProfile(p => 
                {
                    p.CollectionNamesAreCamelCasedAndPlural();
                    p.MemberKeysAreCamelCased();
                }))
                .BuildSessionFactory();
        }
    }
}