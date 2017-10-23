using System.Collections.Generic;

namespace BMI.Models
{
    public class FullBmiReport
    {
        public double BmiIndex { get; set; }
        public string BmiCategory { get; set; }
        public double Percentile { get; set; }
        public List<ReportModel> ReportModels { get; set; }
    }
}
