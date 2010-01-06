using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace MongoDB.Framework.Mapping.IdGenerators
{
    [TestFixture]
    public class OidGeneratorTests
    {

        [Test]
        public void Should_generate_a_non_empty_guid()
        {
            var generator = new OidGenerator();
            var oid = generator.Generate(null, null);

            Assert.IsNotNull(oid);
        }
    }
}