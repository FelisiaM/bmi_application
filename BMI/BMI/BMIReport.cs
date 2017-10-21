using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMI
{
    public class BmiReport : IBmiReport
    {
        public double GetBmi(double height, double weight)
        {
            if (height == 0)
            {
                throw new ArgumentException("height");
            }

            if (weight == 0)
            {
                throw new ArgumentException("weight");
            }
            return weight / (height * height);
        }
    }
}
