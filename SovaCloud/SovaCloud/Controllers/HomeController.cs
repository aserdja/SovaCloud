using Microsoft.AspNetCore.Mvc;
using SovaCloud.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SovaCloud.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [Authorize]
        public IActionResult YourFiles()
        {
            return View();
        }

        [Authorize]
        public IActionResult HomeLogined()
        {
            var currentUser = HttpContext.User;
            var userEmail = currentUser.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            return View("HomeLogined", userEmail);
        }
    }
}
