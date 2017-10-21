using System;
using System.Collections.Generic;
using BMI;
using BMI.Models;
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
            underTest.GetBmiIndex(0d, 50);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_ThrowArgumentException_When_WeightIsZero()
        {
            // [arrange]
            var underTest = new BmiReport();
            // [act]
            underTest.GetBmiIndex(150, 0d);
        }

        [TestMethod]
        public void Should_ReturnValidBmiIndex_When_WeightAndHeightAreNotZero()
        {
            // [arrange]
            var height = 1.60;
            var weight = 50d;
            var expectedIndex = 19.5;

            var underTest = new BmiReport();

            // [act]
            var result = underTest.GetBmiIndex(height, weight);

            // [assert]
            Assert.AreEqual(expectedIndex, result);
        }

        [TestMethod]
        public void Should_RoundToOneFloatingPoint_WhenIndexHasMoreThenOneDecimalPlaces()
        {
            // [arrange]
            var height = 1.77;
            var weight = 70d;

            var underTest = new BmiReport();

            // [act]
            var result = underTest.GetBmiIndex(height, weight);

            // [assert]
            //Has only one digit after floating point
            Assert.AreEqual(0, (result * 100) % 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_ThrowArgumentException_When_IndexIsZero()
        {
            // [arrange]
            var underTest = new BmiReport();
            // [act]
            underTest.GetBmiCategory(0d);
        } 

        [TestMethod]
        public void Should_ReturnUnderWeight_When_IndexIsBellow18_5()
        {
            // [arrange]
            var indexList = new[] {18.4, 18.5};

            var underTest = new BmiReport();

            // [act]
            foreach (var index in indexList)
            {
                var result = underTest.GetBmiCategory(index);

                // [assert]
                Assert.AreEqual(BmiCategory.Underweight, result, "Assertion failed for: " + index);
            }
        }

        [TestMethod]
        public void Should_ReturnNormal_When_IndexIsBetween18_6And24_9Inclusive()
        {
            // [arrange]
            var indexList = new[] {18.6, 20, 24.9};

            var underTest = new BmiReport();

            // [act]
            foreach (var index in indexList)
            {
                var result = underTest.GetBmiCategory(index);

                // [assert]
                Assert.AreEqual(BmiCategory.Normal, result, "Assertion failed for: " + index);
            }
        }

        [TestMethod]
        public void Should_ReturnPreobesity_When_IndexIsBetween25nd29_9Inclusive()
        {
            // [arrange]
            var indexList = new[] {25, 27.8, 29.9};

            var underTest = new BmiReport();

            // [act]
            foreach (var index in indexList)
            {
                var result = underTest.GetBmiCategory(index);

                // [assert]
                Assert.AreEqual(BmiCategory.Preobesity, result, "Assertion failed for: " + index);
            }
        }

        [TestMethod]
        public void Should_ReturnObesityClass1_When_IndexIsBetween30nd34_9Inclusive()
        {
            // [arrange]
            var indexList = new[] {30, 32.6, 34.9};

            var underTest = new BmiReport();

            // [act]
            foreach (var index in indexList)
            {
                var result = underTest.GetBmiCategory(index);

                // [assert]
                Assert.AreEqual(BmiCategory.ObesityClass1, result, "Assertion failed for: " + index);
            }
        }

        [TestMethod]
        public void Should_ReturnUndefined_When_IndexIsHigherThen34_9()
        {
            // [arrange]
            var index = 35;

            var underTest = new BmiReport();

            // [act]
            var result = underTest.GetBmiCategory(index);

            // [assert]
            Assert.AreEqual(BmiCategory.Undefined, result, "Assertion failed for: " + index);
        }

        [TestMethod]
        public void Should_GroupByCategory_When_CallingGetRmiReport()
        {
            // [arrange]
            var list = new List<UserDetails>()
            {
                new UserDetails{ Height = 1.50,Weight = 40}, // underweight
                new UserDetails{ Height = 1.55,Weight = 45}, // normal
                new UserDetails{ Height = 1.55,Weight = 60}, // pre-obesity
                new UserDetails{ Height = 1.55,Weight = 80}, // ovesity class 1
                new UserDetails{ Height = 1.55,Weight = 100}  // undefined
            };

            var underTest = new BmiReport();

            // [act]
            var result = underTest.GetBmiPopulationReport(list);

            // [assert]
            Assert.AreEqual(1, result.GetValueOrDefault(BmiCategory.Underweight), "Assertion failed for " + BmiCategory.Underweight);
            Assert.AreEqual(1, result.GetValueOrDefault(BmiCategory.Normal), "Assertion failed for " + BmiCategory.Normal);
            Assert.AreEqual(1, result.GetValueOrDefault(BmiCategory.Preobesity), "Assertion failed for " + BmiCategory.Preobesity);
            Assert.AreEqual(1, result.GetValueOrDefault(BmiCategory.ObesityClass1), "Assertion failed for " + BmiCategory.ObesityClass1);
            Assert.AreEqual(1, result.GetValueOrDefault(BmiCategory.Undefined), "Assertion failed for " + BmiCategory.Undefined);
        }

        [TestMethod]
        public void Should_GroupByResultsCategory_When_CallingGetRmiReport()
        {
            // [arrange]
            var list = new List<UserDetails>()
            {
                new UserDetails{ Height = 1.50,Weight = 40}, // underweight
                new UserDetails{ Height = 1.50,Weight = 40}, // underweight

                new UserDetails{ Height = 1.55,Weight = 45}, // normal

                new UserDetails{ Height = 1.55,Weight = 60}, // pre-obesity
                new UserDetails{ Height = 1.55,Weight = 60}, // pre-obesity
                new UserDetails{ Height = 1.55,Weight = 60}, // pre-obesity

                new UserDetails{ Height = 1.55,Weight = 80}, // ovesity class 1
            };

            var underTest = new BmiReport();

            // [act]
            var result = underTest.GetBmiPopulationReport(list);

            // [assert]
            Assert.AreEqual(2, result.GetValueOrDefault(BmiCategory.Underweight), "Assertion failed for " + BmiCategory.Underweight);
            Assert.AreEqual(1, result.GetValueOrDefault(BmiCategory.Normal), "Assertion failed for " + BmiCategory.Normal);
            Assert.AreEqual(3, result.GetValueOrDefault(BmiCategory.Preobesity), "Assertion failed for " + BmiCategory.Preobesity);
            Assert.AreEqual(1, result.GetValueOrDefault(BmiCategory.ObesityClass1), "Assertion failed for " + BmiCategory.ObesityClass1);
            Assert.AreEqual(0, result.GetValueOrDefault(BmiCategory.Undefined), "Assertion failed for " + BmiCategory.Undefined);
        }
    }
}
