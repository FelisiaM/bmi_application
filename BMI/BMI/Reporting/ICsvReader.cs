using System.Collections.Generic;
using BMI.Models;

namespace BMI.Reporting
{
    public interface ICsvReader
    {
        List<UserDetails> ReadCsv(string csvPath);
    }
}