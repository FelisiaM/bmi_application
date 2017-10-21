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
            ViewData["User"] = details.Name;
            if (!ModelState.IsValid)
            {
                return View();
            }

            ViewData["BMI"] = _bmiReport.GetBmi(details.Height, details.Weight);
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
