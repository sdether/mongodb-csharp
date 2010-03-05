using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace MongoDB.Framework.Configuration
{
    [TestFixture]
    public class FluentConfigurationTests
    {
        [Test]
        public void Should_configure()
        {
            var sessionFactory = Fluently.Configure()
                .Database("tests")
                .WithAutoMappingProfile(c => 
                {
                    c.CollectionNamesAreCamelCasedAndPlural();
                    c.MemberKeysAreCamelCased();
                })
                .BuildSessionFactory();
        }
    }
}