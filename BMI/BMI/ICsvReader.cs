using System.Collections.Generic;
using BMI.Models;

namespace BMI
{
    public interface ICsvReader
    {
        List<UserDetails> ReadCsv(string csvPath);
    }
}