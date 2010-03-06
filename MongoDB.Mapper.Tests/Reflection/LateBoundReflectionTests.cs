using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace MongoDB.Mapper.Reflection
{
    [TestFixture]
    public class LateBoundReflectionTests
    {
        private class TestClass
        {
            private string propertyReadOnly = "PropertyReadOnly";
            public string propertyWriteOnly = "PropertyWriteOnly";

            public readonly string FieldInitOnly = "FieldInitOnly";

            public string Field = "Field";

            public string PropertyReadOnly { get { return propertyReadOnly; } }

            public string Property { get; set; }

            public string PropertyWriteOnly { set { propertyWriteOnly = value; } }

            public TestClass()
            {
                Property = "Property";
            }
        }

        [Test]
        public void Field_getter()
        {
            var getter = LateBoundReflection.GetGetter(typeof(TestClass).GetField("Field"));
            Assert.IsNotNull(getter);

            object value = getter(new TestClass());
            Assert.AreEqual("Field", value);
        }

        [Test]
        public void Field_setter()
        {
            var setter = LateBoundReflection.GetSetter(typeof(TestClass).GetField("Field"));
            Assert.IsNotNull(setter);

            var testClass = new TestClass();
            setter(testClass, "SomethingElse");
            Assert.AreEqual("SomethingElse", testClass.Field);
        }

        [Test]
        public void Property_getter()
        {
            var getter = LateBoundReflection.GetGetter(typeof(TestClass).GetProperty("Property"));
            Assert.IsNotNull(getter);

            object value = getter(new TestClass());
            Assert.AreEqual("Property", value);
        }

        [Test]
        public void Property_setter()
        {
            var setter = LateBoundReflection.GetSetter(typeof(TestClass).GetProperty("Property"));
            Assert.IsNotNull(setter);

            var testClass = new TestClass();
            setter(testClass, "SomethingElse");
            Assert.AreEqual("SomethingElse", testClass.Property);
        }

    }
}
