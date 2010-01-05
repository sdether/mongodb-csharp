using System;
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
            private IMongoSessionImplementor mongoSession;

            [SetUp]
            public void SetUp()
            {
                mongoSession = new Mock<IMongoSessionImplementor>().Object;
            }

            [Test]
            public void should_return_MongoDBNull_when_value_is_null()
            {
                var valueType = new GuidValueType();
                var result = valueType.ConvertToDocumentValue(null, mongoSession);

                Assert.AreEqual(MongoDBNull.Value, result);
            }

            [Test]
            public void should_return_string_when_value_is_not_null()
            {
                var valueType = new GuidValueType();
                var result = valueType.ConvertToDocumentValue(Guid.Empty, mongoSession);

                Assert.AreEqual(Guid.Empty.ToString(), result);
            }
        }

        [TestFixture]
        public class When_converting_from_a_document
        {
            private IMongoSessionImplementor mongoSession;

            [SetUp]
            public void SetUp()
            {
                mongoSession = new Mock<IMongoSessionImplementor>().Object;
            }

            [Test]
            public void should_return_an_empty_guid_when_value_is_null()
            {
                var valueType = new GuidValueType();
                var result = valueType.ConvertFromDocumentValue(null, mongoSession);

                Assert.AreEqual(Guid.Empty, result);
            }

            [Test]
            public void should_return_a_guid_when_value_is_valid()
            {
                var guid = Guid.NewGuid();
                var valueType = new GuidValueType();
                var result = valueType.ConvertFromDocumentValue(guid.ToString(), mongoSession);

                Assert.AreEqual(guid, result);
            }
        }
    }
}