using FluentAssertions;
using QvaCar.Seedwork.Domain;
using System;
using Xunit;

namespace QvaCar.Domain.UnitTests.Common
{
    public class EnumerationTests
    {
        #region Equals
        [Fact]
        public void Equals_When_Id_Is_Equal_But_Different_Type_Returns_False()
        {
            var obj1 = TestEnumeration1.T1V1;
            var obj2 = TestEnumeration1.T1V2;

            bool result = obj1.Equals(obj2);

            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_When_Same_Reference_Returns_True()
        {
            bool result = TestEnumeration1.T1V1.Equals(TestEnumeration1.T1V1);
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_Same_Id_True_Even_If_Value_Is_Different()
        {
            var obj1 = TestEnumeration1.T1V1;
            var obj2 = new TestEnumeration1(1, "SS");

            bool result = obj1.Equals(obj2);

            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_When_Same_Id_Are_Diferent_False()
        {
            var obj1 = TestEnumeration1.T1V1;
            var obj2 = TestEnumeration1.T1V2;

            bool result = obj1.Equals(obj2);

            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_When_Other_Is_Not_Enum_False()
        {
            var obj1 = TestEnumeration1.T1V1;
            var obj2 = NonEnumerationClass.T3V1;

            bool result = obj1.Equals(obj2);

            result.Should().BeFalse();
        }
        #endregion

        #region From Id
        [Fact]
        public void Can_Map_From_Id()
        {
            var obj1 = Enumeration.FromValue<TestEnumeration1>(1);

            bool result = obj1.Equals(TestEnumeration1.T1V1);

            result.Should().BeTrue();
        }

        [Fact]
        public void Map_From_Id_Throws_Exception_When_Not_Found()
        {
            Action action = () => Enumeration.FromValue<TestEnumeration1>(10);

            action.Should().Throw<InvalidOperationException>();
        }
        #endregion

        #region From Name
        [Fact]
        public void Can_Map_From_Name()
        {
            var obj1 = Enumeration.FromDisplayName<TestEnumeration1>("Value1");

            bool result = obj1.Equals(TestEnumeration1.T1V1);

            result.Should().BeTrue();
        }

        [Fact]
        public void Map_From_Name_Throws_Exception_When_Not_Found()
        {
            Action action = () => Enumeration.FromDisplayName<TestEnumeration1>("InvalidValue");

            action.Should().Throw<InvalidOperationException>();
        }
        #endregion

        #region Get All
        [Fact]
        public void Can_Get_All()
        {
            var values = Enumeration.GetAll<TestEnumeration1>();

            values.Should().NotBeNull().And.HaveCount(2);
            values.Should().Contain(TestEnumeration1.T1V1);
            values.Should().Contain(TestEnumeration1.T1V2);
        }

        [Fact]
        public void Can_Get_All_When_Empty_Works()
        {
            var values = Enumeration.GetAll<EmptyEnumerationClass>();
            values.Should().NotBeNull().And.BeEmpty();
        }
        #endregion

        #region Enumerations
        private class TestEnumeration1 : Enumeration
        {
            public static readonly TestEnumeration1 T1V1 = new(1, "Value1");
            public static readonly TestEnumeration1 T1V2 = new(2, "Value2");
            public TestEnumeration1(int id, string name) : base(id, name) { }
        }

        private class TestEnumeration2 : Enumeration
        {
            public static readonly TestEnumeration2 T2V1 = new(1, "Value1");
            public static readonly TestEnumeration2 T2V2 = new(2, "Value2");
            protected TestEnumeration2(int id, string name) : base(id, name) { }
        }
        private class NonEnumerationClass
        {
            public int Id { get; private set; }
            public string Name { get; private set; }
            public static readonly NonEnumerationClass T3V1 = new(1, "Value1");
            public static readonly NonEnumerationClass T3V2 = new(2, "Value2");
            protected NonEnumerationClass(int id, string name)
            {
                Id = id;
                Name = name;
            }
        }

        private class EmptyEnumerationClass : Enumeration
        {
            protected EmptyEnumerationClass(int id, string name) : base(id, name) { }
        }

        #endregion
    }
}
