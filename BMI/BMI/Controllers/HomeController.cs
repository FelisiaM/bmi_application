using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BMI.Models;
using Microsoft.Extensions.Logging;

namespace BMI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBmiReport _bmiReport;
        private readonly ICsvReader _csvReader;

        public HomeController(
            IBmiReport bmiReport, 
            ICsvReader csvReader)
        {
            _bmiReport = bmiReport;
            _csvReader = csvReader;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult GuestUser()
        {
            return View("UserInput");
        }

        [HttpPost]
        public IActionResult GuestUser(UserDetails details)
        {
            
            if (!ModelState.IsValid)
            {
                return View();
            }

            var bmiIndex = _bmiReport.GetBmiIndex(details.Height, details.Weight);
            var bmiCategory = _bmiReport.GetBmiCategory(bmiIndex);

            if (bmiCategory.Equals(BmiCategory.Undefined))
            {
                ViewData["Result"] = "Could not define your BMI category.";
            }
            ViewData["Result"] = "Your BMI Index is " + bmiIndex + " and BMI Category is " + bmiCategory;

            var population = TryParseCsv();
            if (population.Any())
            {
                var report = _bmiReport.GetBmiPopulationReport(population);

                ViewData["PopulationReport"] = report
                    .Select(o => new ReportModel
                        {
                            Category = o.Key,
                            Count = o.Value
                        })
                    .ToList();
            }

            return View();
        }

        private List<UserDetails> TryParseCsv()
        {
            var population = new List<UserDetails>();
            try
            {
                population = _csvReader.ReadCsv(@"BMI.csv");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldnt read csv.");
            }
            return population;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
