using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BMI.Authorisation;
using Microsoft.AspNetCore.Mvc;
using BMI.Models;
using BMI.Reporting;
using Microsoft.AspNetCore.Authorization;

namespace BMI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBmiReport _bmiReport;
        private readonly ICsvReader _csvReader;
        private readonly ITokenHandler _tokenHandler;

        public HomeController(
            IBmiReport bmiReport, 
            ICsvReader csvReader, 
            ITokenHandler tokenHandler)
        {
            _bmiReport = bmiReport;
            _csvReader = csvReader;
            _tokenHandler = tokenHandler;
        }
        
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Index(CancellationToken token)
        {
            try
            {
                Request.Form.TryGetValue("id_token", out var idToken);

                var jwtToken = _tokenHandler.GetJwtSecurityToken(idToken);
                if (!_tokenHandler.IsAuthorised(jwtToken))
                {
                    return View("UserInput");
                }
                
                ViewData["UserName"] = _tokenHandler.GetUserName(jwtToken);
                BuildBMIReport(
                    _tokenHandler.GetUserDetailsFromClaims(jwtToken));

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failure happen during validating id_token.");
            }

            return View("UserInput");
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

            BuildBMIReport(details);

            return View();
        }

        private void BuildBMIReport(UserDetails details)
        {
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

                var usersRanking = _bmiReport.GetUsersPercentile(population, bmiIndex);

                ViewData["UsersRanking"] = usersRanking;
            }
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
    }
}
