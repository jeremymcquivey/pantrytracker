using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantryTracker.Model.Extensions;

namespace PantryTrackers.Model.Tests
{
    [TestClass]
    public class ParsingFractionTests
    {
        [TestMethod]
        public void DoesNotThrowExceptionWithDivideByZero()
        {
            double expected = 0;
            var fraction = "3/0";

            Assert.AreEqual(expected, fraction.ToNumber());
        }

        [TestMethod]
        public void DoesNotThrowExceptionWithAlphaNumeric()
        {
            double expected = 0;
            var input = "123ABC";

            Assert.AreEqual(expected, input.ToNumber());
        }

        [TestMethod]
        //TODO: This case may be supported in the future as a "range".
        public void DoesNotThrowExceptionWithMultiple()
        {
            double expected = 0;
            var input = "1 1";

            Assert.AreEqual(expected, input.ToNumber());
        }

        [TestMethod]
        //TODO: This case may be supported in the future as a "range".
        public void DoesNotThrowExceptionWithWholeNumberAfterFraction()
        {
            double expected = 0;
            var input = "1/2 1";

            Assert.AreEqual(expected, input.ToNumber());
        }

        [TestMethod]
        public void FractionWithWholeNumber()
        {
            double expected = 1.5;
            var input = "1 1/2";

            Assert.AreEqual(expected, input.ToNumber());
        }

        [TestMethod]
        public void StandardFractionWithLeftPadding()
        {
            double expected = .5;
            var input = "    1/2";

            Assert.AreEqual(expected, input.ToNumber());
        }

        [TestMethod]
        public void StandardFractionWithRightPadding()
        {
            double expected = .5;
            var input = "1/2       ";

            Assert.AreEqual(expected, input.ToNumber());
        }
        
        [TestMethod]
        public void FractionWithInternalPadding()
        {
            double expected = .5;
            var input = "1 / 2";
        }

        [TestMethod]
        public void ImproperFraction()
        {
            double expected = 3.2;
            var input = "16/5";

            Assert.AreEqual(expected, input.ToNumber());
        }

        [TestMethod]
        public void BasicDecimal()
        {
            double expected = 38.58;
            var input = "38.58";

            Assert.AreEqual(expected, input.ToNumber());
        }

        [TestMethod]
        public void BasicWholeNumber()
        {
            double expected = 38;
            var input = "38";

            Assert.AreEqual(expected, input.ToNumber());
        }
    }
}