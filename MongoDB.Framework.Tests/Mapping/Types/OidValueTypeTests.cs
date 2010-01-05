using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;

using Moq;
using NUnit.Framework;

namespace MongoDB.Framework.Mapping.Types
{
    public class OidValueTypeTests
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
                var valueType = new OidValueType();
                var result = valueType.ConvertToDocumentValue(null, mongoSession);

                Assert.AreEqual(MongoDBNull.Value, result);
            }

            [Test]
            public void should_return_Oid_when_value_is_not_null()
            {
                var valueType = new OidValueType();
                var result = valueType.ConvertToDocumentValue("f7f6ec027e6c63440b000000", mongoSession);

                Assert.AreEqual(new Oid("f7f6ec027e6c63440b000000"), result);
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
            public void should_return_null_when_value_is_null()
            {
                var valueType = new OidValueType();
                var result = valueType.ConvertFromDocumentValue(null, mongoSession);

                Assert.IsNull(result);
            }

            [Test]
            public void should_return_a_string_when_value_is_valid()
            {
                var valueType = new OidValueType();
                var result = valueType.ConvertFromDocumentValue(new Oid("f7f6ec027e6c63440b000000"), mongoSession);

                Assert.AreEqual("f7f6ec027e6c63440b000000", result);
            }
        }
    }
}