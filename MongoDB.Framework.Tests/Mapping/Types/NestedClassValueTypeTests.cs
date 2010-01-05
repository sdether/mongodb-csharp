using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Framework.Reflection;

using Moq;
using NUnit.Framework;

namespace MongoDB.Framework.Mapping.Types
{
    public class NestedClassValueTypeTests
    {
        public class Complex
        {
            public int Real { get; set; }
            public int Imaginary { get; set; }
        }

        private static NestedClassMap GetComplexNestedClassMap()
        {
            var realMemberMap = new MemberMap("Real",
                "Real",
                LateBoundReflection.GetGetter(typeof(Complex).GetProperty("Real")),
                LateBoundReflection.GetSetter(typeof(Complex).GetProperty("Real")),
                new NullSafeValueType(typeof(int)));

            var imaginaryMemberMap = new MemberMap("Imaginary",
                "Imaginary",
                LateBoundReflection.GetGetter(typeof(Complex).GetProperty("Imaginary")),
                LateBoundReflection.GetSetter(typeof(Complex).GetProperty("Imaginary")),
                new NullSafeValueType(typeof(int)));

            return new NestedClassMap(typeof(Complex),
                new[] { realMemberMap, imaginaryMemberMap },
                Enumerable.Empty<ManyToOneMap>(),
                null,
                null,
                null,
                null);
        }

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
                var valueType = new NestedClassValueType(GetComplexNestedClassMap());
                var result = valueType.ConvertToDocumentValue(null, mongoSession);

                Assert.AreEqual(MongoDBNull.Value, result);
            }

            [Test]
            public void should_return_a_document_when_value_is_not_null()
            {
                var valueType = new NestedClassValueType(GetComplexNestedClassMap());
                var result = (Document)valueType.ConvertToDocumentValue(new Complex() { Real = 24, Imaginary = 42 }, mongoSession);

                Assert.AreEqual(24, result["Real"]);
                Assert.AreEqual(42, result["Imaginary"]);
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
                var valueType = new NestedClassValueType(GetComplexNestedClassMap());
                var result = valueType.ConvertFromDocumentValue(null, mongoSession);

                Assert.IsNull(result);
            }

            [Test]
            public void should_return_an_instance_when_value_is_valid()
            {
                var valueType = new NestedClassValueType(GetComplexNestedClassMap());
                var result = (Complex)valueType.ConvertFromDocumentValue(new Document().Append("Real", 24).Append("Imaginary", 42), mongoSession);

                Assert.AreEqual(24, result.Real);
                Assert.AreEqual(42, result.Imaginary);
            }
        }
    }
}