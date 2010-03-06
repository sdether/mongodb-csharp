using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using NUnit.Framework;

namespace MongoDB.Mapper.Linq.Visitors
{
    [TestFixture]
    public class MemberInfoPathExpressionTreeVisitorTests
    {
        private class TestClass
        {
            public string Field = null;

            public string Prop { get; set; }

            public NestedTestClass Nested { get; set; }
        }

        private class NestedTestClass
        {
            public string NestedProp { get; set; }
        }


        [Test]
        public void Should_get_field_member()
        {
            //Act
            var members = MemberInfoPathBuilder.BuildFrom((Expression<Func<TestClass, string>>)((TestClass tc) => tc.Field));

            //Assert
            Assert.AreEqual(MemberTypes.Field, members[0].MemberType);
        }

        [Test]
        public void Should_get_property_member()
        {
            //Act
            var members = MemberInfoPathBuilder.BuildFrom((Expression<Func<TestClass, string>>)((TestClass tc) => tc.Prop));

            //Assert
            Assert.AreEqual(MemberTypes.Property, members[0].MemberType);
        }

        [Test]
        public void Should_get_chain_of_property_members()
        {
            //Act
           var members = MemberInfoPathBuilder.BuildFrom((Expression<Func<TestClass, string>>)((TestClass tc) => tc.Nested.NestedProp));

            //Assert
            Assert.AreEqual(2, members.Count);
            Assert.AreEqual("Nested", members[0].Name);
            Assert.AreEqual("NestedProp", members[1].Name);
        }
    }
}