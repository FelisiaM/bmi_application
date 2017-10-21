using System.Collections.Generic;
using BMI.Models;

namespace BMI
{
    public interface IBmiReport
    {
        double GetBmiIndex(double height, double weight);
        string GetBmiCategory(double index);
        Dictionary<string, int> GetBmiPopulationReport(List<UserDetails> measurmentsList);
        double GetUsersPercentile(List<UserDetails> measurmentsList, double userToFind);
    }
}