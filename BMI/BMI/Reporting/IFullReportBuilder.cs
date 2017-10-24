using BMI.Models;

namespace BMI.Reporting
{
    public interface IFullReportBuilder
    {
        FullBmiReport BuildBmiReport(UserDetails details);
    }
}