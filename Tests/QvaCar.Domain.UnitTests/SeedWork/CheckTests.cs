using FluentAssertions;
using QvaCar.Seedwork.Domain;
using System;
using Xunit;

namespace QvaCar.Domain.UnitTests.SeedWork
{
    public class CheckTest
    {
        #region Is Not Null
        [Fact]
        public void IsNotNull_Does_Not_Throws_Exception_When_True()
        {
            Action act = () => Check.IsNotNull<TestException>(1);
            act.Should().NotThrow<TestException>();
        }

        [Fact]
        public void IsNotNull_Throws_Exception_When_False()
        {
            Action act = () => Check.IsNotNull<TestException>(null);
            act.Should().Throw<TestException>();
        }

        [Fact]
        public void IsNotNull_Throws_Exception_When_False_With_Custom_Message()
        {
            var errorMsg = "err";
            TestException? exGlobal = null;

            try
            {
                Check.IsNotNull<TestException>(null, errorMsg);
            }
            catch (TestException ex)
            { exGlobal = new TestException(ex.Message); }

            exGlobal.Should().NotBeNull();
            exGlobal?.Message?.Should().Be(errorMsg);
        }

        [Fact]
        public void IsNotNull_Does_Not_Throws_Exception_When_True_With_Custom_Message()
        {
            Action act = () => Check.IsNotNull<TestException>(1, "err");
            act.Should().NotThrow<TestException>();
        }
        #endregion

        #region That
        [Fact]

        public void That_Does_Not_Throws_Exception_When_True()
        {
            Action act = () => Check.That<TestException>(true);
            act.Should().NotThrow<TestException>();
        }

        [Fact]
        public void That_Throws_Exception_When_False()
        {
            Action act = () => Check.That<TestException>(false);
            act.Should().Throw<TestException>();
        }
        [Fact]
        public void That_Throws_Exception_When_False_With_Custom_Message()
        {
            var errorMsg = "err";
            TestException? exGlobal = null;

            try
            {
                Check.That<TestException>(false, errorMsg);
            }
            catch (TestException ex)
            { exGlobal = new TestException(ex.Message); }

            exGlobal.Should().NotBeNull();
            exGlobal?.Message?.Should().Be(errorMsg);
        }

        [Fact]
        public void That_Configure_Exception_Does_Not_Throws_Exception_When_True()
        {
            Action act = () => Check.That<TestException>(true, () => new TestException());
            act.Should().NotThrow<TestException>();
        }

        [Fact]
        public void That_Configure_Exception_Throws_Exception_When_False()
        {
            Action act = () => Check.That<TestException>(false, () => new TestException());
            act.Should().Throw<TestException>();
        }
        [Fact]
        public void That_Configure_Exception_Throws_Exception_When_False_With_Custom_Message()
        {
            var errorMsg = "err";
            TestException? exGlobal = null;

            try
            {
                Check.That<TestException>(false, () => new TestException(errorMsg));
            }
            catch (TestException ex)
            { exGlobal = new TestException(ex.Message); }

            exGlobal.Should().NotBeNull();
            exGlobal?.Message?.Should().Be(errorMsg);
        }
        #endregion

        private class TestException : Exception
        {
            public TestException() : this("") { }
            public TestException(string message) : base(message) { }
        }
    }
}
