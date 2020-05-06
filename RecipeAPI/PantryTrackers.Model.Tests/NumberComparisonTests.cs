using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantryTracker.Model.Extensions;

namespace PantryTrackers.Model.Tests
{
    [TestClass]
    public class NumberComparisonTests
    {
        [TestMethod]
        public void IsGreaterThanOrEqualTo()
        {
            var lowerNumber = 5D;
            var higherNumber = 10D;

            Assert.IsTrue(higherNumber.IsGreaterThanOrEqualTo(lowerNumber, 1));
        }

        [TestMethod]
        public void IsNotGreaterThanOrEqualTo()
        {
            var lowerNumber = 5D;
            var higherNumber = 10D;

            Assert.IsFalse(lowerNumber.IsGreaterThanOrEqualTo(higherNumber, 1));
        }

        [TestMethod]
        public void IsNotGreaterThanOrEqualToZero()
        {
            var lowerNumber = 0D;
            var higherNumber = .1D;
            var scale = .5D;

            Assert.IsFalse(higherNumber.IsGreaterThanOrEqualTo(lowerNumber, scale));
        }

        [TestMethod]
        public void IsGreaterThanOrEqualToZero()
        {
            var lowerNumber = 0D;
            var higherNumber = .6D;
            var scale = .5D;

            Assert.IsTrue(higherNumber.IsGreaterThanOrEqualTo(lowerNumber, scale));
        }

        [TestMethod]
        public void IsEqualToZero()
        {
            var lowerNumber = 0D;
            var higherNumber = .5D;
            var scale = .5D;

            Assert.IsTrue(higherNumber.IsGreaterThanOrEqualTo(lowerNumber, scale));
        }

        [TestMethod]
        public void IsApproximatelyEqual()
        {
            var lowerNumber = 0D;
            var higherNumber = .4D;
            var scale = .5D;

            Assert.IsTrue(higherNumber.IsApproximatelyEqual(lowerNumber, scale));
        }

        [TestMethod]
        public void IsNotApproximatelyEqual()
        {
            var lowerNumber = 0D;
            var higherNumber = .55D;
            var scale = .5D;

            Assert.IsFalse(higherNumber.IsApproximatelyEqual(lowerNumber, scale));
        }

        [TestMethod]
        public void IsNotEvenCloseToApproximatelyEqual()
        {
            var lowerNumber = 0D;
            var higherNumber = 55D;
            var scale = .5D;

            Assert.IsFalse(higherNumber.IsApproximatelyEqual(lowerNumber, scale));
        }
    }
}
