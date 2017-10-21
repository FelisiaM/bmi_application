using System.Collections.Generic;
using System.IO;
using BMI.Models;

namespace BMI
{
    public class CsvReader : ICsvReader
    {
        public List<UserDetails> ReadCsv(string csvPath)
        {
            var measurmentList = new List<UserDetails>();

            using (var reader = new StreamReader(csvPath))
            {
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine(); 
                    var values = line.Split(',');

                    measurmentList.Add(new UserDetails()
                    {
                        Height = double.Parse(values[0]),
                        Weight = double.Parse(values[1])
                    });
                }
            }

            return measurmentList;
        }
    }
}
