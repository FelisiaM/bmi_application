using System;
using System.Threading;
using BMI.Authorisation;
using Microsoft.AspNetCore.Mvc;
using BMI.Models;
using BMI.Reporting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;

namespace BMI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITokenHandler _tokenHandler;
        private readonly IFullReportBuilder _fullReportBuilder;

        public HomeController(
            ITokenHandler tokenHandler,
            IFullReportBuilder fullReportBuilder)
        {
            _tokenHandler = tokenHandler;
            _fullReportBuilder = fullReportBuilder;
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

                var userDetailsFromClaims = _tokenHandler.GetUserDetailsFromClaims(jwtToken);
                var fullBmiReport = _fullReportBuilder.BuildBmiReport(userDetailsFromClaims);

                return View(fullBmiReport);
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

            var fullBmiReport = _fullReportBuilder.BuildBmiReport(details);

            return View("BmiReport" , fullBmiReport);
        }

        public IActionResult Signout()
        {
            return SignOut(
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
