using System;
using BMI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BMI_Tests
{
    [TestClass]
    public class BmiReportTests
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_ThrowArgumentException_When_HeightIsZero()
        {
            // [arrange]
            var underTest = new BmiReport();
            // [act]
            underTest.GetBmi(0d, 50);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_ThrowArgumentException_When_WeightIsZero()
        {
            // [arrange]
            var underTest = new BmiReport();
            // [act]
            underTest.GetBmi(150, 0d);
        }

        [TestMethod]
        public void Should_ReturnValidBmiIndex_When_WeightAndHeightAreNotZero()
        {
            // [arrange]
            var height = 1.60;
            var weight = 50d;

            var underTest = new BmiReport();

            // [act]
            var result = underTest.GetBmi(height, weight);

            // [assert]
            Assert.AreEqual(weight / (height * height), result);
        }
    }
}
