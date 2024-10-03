using Microsoft.AspNetCore.Mvc;
using PoeSample.Services;

namespace PoeSample.Controllers
{
    public class LecturerController1 : Controller
    {
        public IActionResult Index()
        {
            ClaimService claimService = new ClaimService();
            var claims = claimService.GetAllClaimsForUser(1);//get the profile that is logged in
            return View(claims);
        }
    }
}
