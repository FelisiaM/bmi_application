using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BMI.Models;

namespace BMI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBmiReport _bmiReport;

        public HomeController(IBmiReport bmiReport)
        {
            _bmiReport = bmiReport;
        }
        
        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult GuestUser()
        {
            return View();
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
            ViewData["Result"] = "Your BMI Index is " + bmiIndex + "and BMI Category is " + bmiCategory;

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
