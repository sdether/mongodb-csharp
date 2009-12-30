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
            [Test]
            public void should_convert_nulls_into_MongoDBNull()
            {
                var valueType = new NullSafeValueType(typeof(string));
                var result = valueType.ConvertToDocumentValue(null);

                Assert.AreEqual(MongoDBNull.Value, result);
            }

            [Test]
            public void should_leave_value_alone_when_not_null()
            {
                var valueType = new NullSafeValueType(typeof(string));
                var result = valueType.ConvertToDocumentValue("Sammy");

                Assert.AreEqual("Sammy", result);
            }
        }

        [TestFixture]
        public class When_converting_from_a_document
        {
            [TestFixture]
            public class Given_the_type_is_a_reference_type
            {
                private IMappingContext mappingContext;

                [SetUp]
                public void SetUp()
                {
                    mappingContext = new Mock<IMappingContext>().Object;
                }

                [Test]
                public void should_convert_null_into_null()
                {
                    var valueType = new NullSafeValueType(typeof(Uri));
                    var result = valueType.ConvertFromDocumentValue(null, mappingContext);

                    Assert.IsNull(result);
                }

                [Test]
                public void should_convert_MongoDBNull_into_null()
                {
                    var valueType = new NullSafeValueType(typeof(Uri));
                    var result = valueType.ConvertFromDocumentValue(MongoDBNull.Value, mappingContext);

                    Assert.IsNull(result);
                }

                [Test]
                public void should_leave_value_alone_when_not_null()
                {
                    var valueType = new NullSafeValueType(typeof(Uri));
                    var result = valueType.ConvertFromDocumentValue(new Uri("http://localhost"), mappingContext);

                    Assert.AreEqual(new Uri("http://localhost"), result);
                }
            }

            [TestFixture]
            public class Given_the_type_is_a_value_type
            {
                private IMappingContext mappingContext;

                [SetUp]
                public void SetUp()
                {
                    mappingContext = new Mock<IMappingContext>().Object;
                }

                [Test]
                public void should_convert_null_into_null()
                {
                    var valueType = new NullSafeValueType(typeof(int));
                    var result = valueType.ConvertFromDocumentValue(null, mappingContext);

                    Assert.AreEqual(0, result);
                }

                [Test]
                public void should_convert_MongoDBNull_into_null()
                {
                    var valueType = new NullSafeValueType(typeof(int));
                    var result = valueType.ConvertFromDocumentValue(MongoDBNull.Value, mappingContext);

                    Assert.AreEqual(0, result);
                }

                [Test]
                public void should_leave_value_alone_when_not_null()
                {
                    var valueType = new NullSafeValueType(typeof(int));
                    var result = valueType.ConvertFromDocumentValue(42, mappingContext);

                    Assert.AreEqual(42, result);
                }
            }
        }
    }
}