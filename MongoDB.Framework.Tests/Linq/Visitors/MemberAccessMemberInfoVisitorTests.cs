using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using NUnit.Framework;

namespace MongoDB.Framework.Linq.Visitors
{
    [TestFixture]
    public class MemberAccessMemberInfoVisitorTests
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
            //Arrange
            var visitor = new MemberAccessMemberInfoVisitor();

            //Act
            visitor.Visit((Expression<Func<TestClass, string>>)((TestClass tc) => tc.Field));

            //Assert
            Assert.AreEqual(MemberTypes.Field, visitor.Members.First().MemberType);
        }

        [Test]
        public void Should_get_property_member()
        {
            //Arrange
            var visitor = new MemberAccessMemberInfoVisitor();

            //Act
            visitor.Visit((Expression<Func<TestClass, string>>)((TestClass tc) => tc.Prop));

            //Assert
            Assert.AreEqual(MemberTypes.Property, visitor.Members.First().MemberType);
        }

        [Test]
        public void Should_get_chain_of_property_members()
        {
            //Arrange
            var visitor = new MemberAccessMemberInfoVisitor();

            //Act
            visitor.Visit((Expression<Func<TestClass, string>>)((TestClass tc) => tc.Nested.NestedProp));

            //Assert
            Assert.AreEqual(2, visitor.Members.Count());
            Assert.AreEqual("Nested", visitor.Members.First().Name);
            Assert.AreEqual("NestedProp", visitor.Members.Last().Name);
        }
    }
}