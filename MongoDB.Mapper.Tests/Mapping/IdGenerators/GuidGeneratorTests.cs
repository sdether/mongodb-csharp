﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace MongoDB.Mapper.Mapping.IdGenerators
{
    [TestFixture]
    public class GuidGeneratorTests
    {

        [Test]
        public void Should_generate_a_non_empty_guid()
        {
            var generator = new GuidGenerator();
            var guid = (Guid)generator.Generate(null, null);

            Assert.AreNotEqual(Guid.Empty, guid);
        }
    }
}