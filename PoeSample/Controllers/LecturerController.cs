using Microsoft.AspNetCore.Mvc;
using PoeSample.Models;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using PoeSample.Services;

namespace PoeSample.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ClaimsContext claimsContext;
        ClaimService claimService;
        public LecturerController(ILogger<HomeController> logger)
        {
            _logger = logger;
            claimsContext = new ClaimsContext();
            claimsContext.Database.EnsureCreated();//force seeding of data
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
            var claims = claimService.GetAllClaimsForUser(userId.Value);//get the profile that is logged in
            return View(claims);
        }

        public IActionResult AddClaim()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddClaim(ClaimViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                var claim = new Claim();
                claim.PersonId = userId ?? 0;
                claim.Hours = model.Hours;
                claim.CourseId = model.CourseId;
                claim.ClassId = model.ClassId;
                claim.StatusId = 3;//Pending status
                claimService.AddNewClaim(claim);
                return RedirectToAction("Index", "Home");

            }
            ViewBag.Message = "Error saving claim details";
            return View();
        }
    }
}
