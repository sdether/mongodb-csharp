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
            private IMappingContext mappingContext;

            [SetUp]
            public void SetUp()
            {
                mappingContext = new Mock<IMappingContext>().Object;
            }

            [Test]
            public void should_return_MongoDBNull_when_value_is_null()
            {
                var valueType = new OidValueType();
                var result = valueType.ConvertToDocumentValue(null, mappingContext);

                Assert.AreEqual(MongoDBNull.Value, result);
            }

            [Test]
            public void should_return_Oid_when_value_is_not_null()
            {
                var valueType = new OidValueType();
                var result = valueType.ConvertToDocumentValue("f7f6ec027e6c63440b000000", mappingContext);

                Assert.AreEqual(new Oid("f7f6ec027e6c63440b000000"), result);
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
            public void should_return_null_when_value_is_null()
            {
                var valueType = new OidValueType();
                var result = valueType.ConvertFromDocumentValue(null, mappingContext);

                Assert.IsNull(result);
            }

            [Test]
            public void should_return_a_string_when_value_is_valid()
            {
                var valueType = new OidValueType();
                var result = valueType.ConvertFromDocumentValue(new Oid("f7f6ec027e6c63440b000000"), mappingContext);

                Assert.AreEqual("f7f6ec027e6c63440b000000", result);
            }
        }
    }
}