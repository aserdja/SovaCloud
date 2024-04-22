using Microsoft.AspNetCore.Mvc;
using SovaCloud.Models;
using System.Diagnostics;

namespace SovaCloud.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult YourFiles()
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
    }
}
