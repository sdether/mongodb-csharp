using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace MongoDB.Framework.Configuration.Mapping.IdGenerators
{
    [TestFixture]
    public class GuidCombGeneratorTests
    {

        [Test]
        public void Should_generate_a_non_empty_guid()
        {
            var generator = new GuidCombGenerator();
            var guid = (Guid)generator.Generate(null, null);

            Assert.AreNotEqual(Guid.Empty, guid);
        }
    }
}