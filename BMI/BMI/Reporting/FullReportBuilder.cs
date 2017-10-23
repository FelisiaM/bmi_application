using System;
using System.Collections.Generic;
using System.Linq;
using BMI.Models;

namespace BMI.Reporting
{
    public class FullReportBuilder : IFullReportBuilder
    {
        private readonly IBmiReport _bmiReport;
        private readonly ICsvReader _csvReader;

        public FullReportBuilder(
            IBmiReport bmiReport,
            ICsvReader csvReader)
        {
            _bmiReport = bmiReport;
            _csvReader = csvReader;
        }

        public FullBmiReport BuildBmiReport(UserDetails details)
        {
            var fullBmiReport = new FullBmiReport
            {
                BmiIndex = _bmiReport.GetBmiIndex(details.Height, details.Weight)
            };
            fullBmiReport.BmiCategory = _bmiReport.GetBmiCategory(fullBmiReport.BmiIndex);
            
            var allUsers = TryParseCsv();
            if (!allUsers.Any())
            {
                return fullBmiReport;
            }

            fullBmiReport.ReportModels = GetReportModels(allUsers);
            fullBmiReport.Percentile = _bmiReport.GetUsersPercentile(allUsers, fullBmiReport.BmiIndex);

            return fullBmiReport;
        }

        private List<ReportModel> GetReportModels(List<UserDetails> allUsers)
        {
            return _bmiReport
                .GetBmiPopulationReport(allUsers)
                .Select(o => new ReportModel
                {
                    Category = o.Key,
                    Count = o.Value
                })
                .ToList();
        }

        private List<UserDetails> TryParseCsv()
        {
            var allUsers = new List<UserDetails>();
            try
            {
                allUsers = _csvReader.ReadCsv(@"BMI.csv");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldnt read csv.");
            }
            return allUsers;
        }
    }
}