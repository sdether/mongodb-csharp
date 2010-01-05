using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;

using Moq;
using NUnit.Framework;

namespace MongoDB.Framework.Mapping.Types
{
    public class NullSafeValueTypeTests
    {
        [TestFixture]
        public class When_converting_to_a_document
        {
            private IMongoContextImplementor mongoContext;

            [SetUp]
            public void SetUp()
            {
                mongoContext = new Mock<IMongoContextImplementor>().Object;
            }

            [Test]
            public void should_convert_nulls_into_MongoDBNull()
            {
                var valueType = new NullSafeValueType(typeof(string));
                var result = valueType.ConvertToDocumentValue(null, mongoContext);

                Assert.AreEqual(MongoDBNull.Value, result);
            }

            [Test]
            public void should_leave_value_alone_when_not_null()
            {
                var valueType = new NullSafeValueType(typeof(string));
                var result = valueType.ConvertToDocumentValue("Sammy", mongoContext);

                Assert.AreEqual("Sammy", result);
            }
        }

        [TestFixture]
        public class When_converting_from_a_document
        {
            [TestFixture]
            public class Given_the_type_is_a_reference_type
            {
                private IMongoContextImplementor mongoContext;

                [SetUp]
                public void SetUp()
                {
                    mongoContext = new Mock<IMongoContextImplementor>().Object;
                }

                [Test]
                public void should_convert_null_into_null()
                {
                    var valueType = new NullSafeValueType(typeof(Uri));
                    var result = valueType.ConvertFromDocumentValue(null, mongoContext);

                    Assert.IsNull(result);
                }

                [Test]
                public void should_convert_MongoDBNull_into_null()
                {
                    var valueType = new NullSafeValueType(typeof(Uri));
                    var result = valueType.ConvertFromDocumentValue(MongoDBNull.Value, mongoContext);

                    Assert.IsNull(result);
                }

                [Test]
                public void should_leave_value_alone_when_not_null()
                {
                    var valueType = new NullSafeValueType(typeof(Uri));
                    var result = valueType.ConvertFromDocumentValue(new Uri("http://localhost"), mongoContext);

                    Assert.AreEqual(new Uri("http://localhost"), result);
                }
            }

            [TestFixture]
            public class Given_the_type_is_a_value_type
            {
                private IMongoContextImplementor mongoContext;

                [SetUp]
                public void SetUp()
                {
                    mongoContext = new Mock<IMongoContextImplementor>().Object;
                }

                [Test]
                public void should_convert_null_into_null()
                {
                    var valueType = new NullSafeValueType(typeof(int));
                    var result = valueType.ConvertFromDocumentValue(null, mongoContext);

                    Assert.AreEqual(0, result);
                }

                [Test]
                public void should_convert_MongoDBNull_into_null()
                {
                    var valueType = new NullSafeValueType(typeof(int));
                    var result = valueType.ConvertFromDocumentValue(MongoDBNull.Value, mongoContext);

                    Assert.AreEqual(0, result);
                }

                [Test]
                public void should_leave_value_alone_when_not_null()
                {
                    var valueType = new NullSafeValueType(typeof(int));
                    var result = valueType.ConvertFromDocumentValue(42, mongoContext);

                    Assert.AreEqual(42, result);
                }
            }
        }
    }
}