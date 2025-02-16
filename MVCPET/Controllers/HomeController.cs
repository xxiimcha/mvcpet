using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVCPET.Models;

namespace MVCPET.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Home page";
            return View();
        }

        public IActionResult Adopt()
        {
            ViewData["Title"] = "Adopt";
            return View();
        }
        public IActionResult Login()
        {
            ViewData["Title"] = "Login";
            return View();
        }
        public IActionResult Register()
        {
            ViewData["Title"] = "Register";
            return View();
        }
        public IActionResult ForgotPassword()
        {
            ViewData["Title"] = "Forgot Password";
            return View();
        }
        public IActionResult PetDetails()
        {
            ViewData["Title"] = "PetDetails";
            return View();
        }
        public IActionResult UserProfile()
        {
            ViewData["Title"] = "User Profile";
            return View();
        }
        public IActionResult PetForm() {
            ViewData["Title"] = "Adoption Form";
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
