﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;

using Moq;
using NUnit.Framework;

namespace MongoDB.Framework.Mapping.Types
{
    public class GuidValueTypeTests
    {
        [TestFixture]
        public class When_converting_to_a_document
        {
            [Test]
            public void should_return_MongoDBNull_when_value_is_null()
            {
                var valueType = new GuidValueType();
                var result = valueType.ConvertToDocumentValue(null);

                Assert.AreEqual(MongoDBNull.Value, result);
            }

            [Test]
            public void should_return_string_when_value_is_not_null()
            {
                var valueType = new GuidValueType();
                var result = valueType.ConvertToDocumentValue(Guid.Empty);

                Assert.AreEqual(Guid.Empty.ToString(), result);
            }
        }

        [TestFixture]
        public class When_converting_from_a_document
        {
            private IMappingContext mappingContext;

            [SetUp]
            public void SetUp()
            {
                mappingContext = new Mock<IMappingContext>().Object;
            }

            [Test]
            public void should_return_an_empty_guid_when_value_is_null()
            {
                var valueType = new GuidValueType();
                var result = valueType.ConvertFromDocumentValue(null, mappingContext);

                Assert.AreEqual(Guid.Empty, result);
            }

            [Test]
            public void should_return_a_guid_when_value_is_valid()
            {
                var guid = Guid.NewGuid();
                var valueType = new GuidValueType();
                var result = valueType.ConvertFromDocumentValue(guid.ToString(), mappingContext);

                Assert.AreEqual(guid, result);
            }
        }
    }
}