using System;

namespace BMI
{
    public class BmiReport : IBmiReport
    {
        public double GetBmiIndex(double height, double weight)
        {
            if (height == 0)
            {
                throw new ArgumentException("height");
            }

            if (weight == 0)
            {
                throw new ArgumentException("weight");
            }

            var index = weight / (height * height);

            return Math.Round(index, 1, MidpointRounding.AwayFromZero);
        }

        public string GetBmiCategory(double index)
        {
            if (index == 0)
            {
                throw new ArgumentException("weight");
            }

            if (index <= 18.5)
            {
                return BmiCategory.Underweight;
            }
            if (index >= 18.6 && index <= 24.9)
            {
                return BmiCategory.Normal;
            }
            if (index >= 25 && index <= 29.9)
            {
                return BmiCategory.Preobesity;
            }
            if (index >= 30 && index <= 34.9)
            {
                return BmiCategory.ObesityClass1;
            }
            return BmiCategory.Undefined;
        }
    }

    public class BmiCategory
    {
        public static string Underweight => "Underweight";
        public static string Normal => "Normal";
        public static string Preobesity => "Pre-obesity";
        public static string ObesityClass1 => "Obesity Class I";
        public static string Undefined => "Undefined";
    }
}
