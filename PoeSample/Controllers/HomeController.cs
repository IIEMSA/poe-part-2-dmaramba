using Microsoft.AspNetCore.Mvc;
using PoeSample.Models;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using PoeSample.Services;

namespace PoeSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ClaimsContext claimsContext;
        ClaimService claimService;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            claimsContext = new ClaimsContext();
            claimsContext.Database.EnsureCreated();
            claimService = new ClaimService();
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            var user = claimsContext.People.FirstOrDefault(x => x.Id == userId);

            ViewBag.UserProfile = user?.FirstName + " " + user?.LastName;

            //get the user's claims from the database
            var claims = claimService.GetAllClaimsForUser(user?.Id ?? 0);
            return View(claims);
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var person = claimsContext.People.FirstOrDefault(x => x.UserName == model.UserName && x.Password == model.Password);
                if (person != null)
                {
                    HttpContext.Session.SetInt32("UserId", person.Id);
                    HttpContext.Session.SetString("Role", person.Role);
                    if (person.Role == "Lecturer")
                    {
                        return RedirectToAction("Index", "Lecturer");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.Message = "Invalid login details";
                    return View();
                }
               
            }
            return View();
        }

     
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
