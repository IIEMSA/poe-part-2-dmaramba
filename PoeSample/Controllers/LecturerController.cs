using Microsoft.AspNetCore.Mvc;
using PoeSample.Services;

namespace PoeSample.Controllers
{
    public class LecturerController : Controller
    {
        public IActionResult Index()
        {
            ClaimService claimService = new ClaimService();
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            var claims = claimService.GetAllClaimsForUser(userId.Value);//get the profile that is logged in
            return View(claims);
        }
    }
}
