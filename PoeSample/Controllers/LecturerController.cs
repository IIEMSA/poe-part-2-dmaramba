using Microsoft.AspNetCore.Mvc;
using PoeSample.Models;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using PoeSample.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PoeSample.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _environment;
        ClaimsContext claimsContext;
        ClaimService claimService;
        DocumentService documentService;
        public LecturerController(ILogger<HomeController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            claimsContext = new ClaimsContext();
            claimsContext.Database.EnsureCreated();//force seeding of data
            claimService = new ClaimService();
            _environment = environment;
            documentService = new DocumentService();
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var user = claimsContext.People.FirstOrDefault(x => x.Id == userId);

            ViewBag.UserProfile = user?.FirstName + " " + user?.LastName;
            var claims = claimService.GetAllClaimsForUser(userId.Value);//get the profile that is logged in
            return View(claims);
        }

        public IActionResult AddClaim()
        {
            var model = new ClaimViewModel();

            var courses = claimsContext.Courses.ToList();
            var classes = claimsContext.Classes.ToList();
            model.Courses = new SelectList(courses, "Id", "Title");
            model.Classes = new SelectList(classes, "Id", "ClassName");
            return View(model);
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
                claim.DateClaimed = DateTime.Now;
                claim.ClassId = model.ClassId;
                claim.StatusId = 3;//Pending status
                int claimId = claimService.AddNewClaim(claim);



                if (model.File != null && model.File.Length > 0)
                {
                    // Define the upload folder
                    string uploadPath = Path.Combine(_environment.WebRootPath, "uploads");
                    // Create the folder if it doesn't exist
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    // Generate the file path
                    string filePath = Path.Combine(uploadPath, model.File.FileName);
                    // Save the file to the specified location
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        model.File.CopyTo(stream);
                    }


                    var document = new ClaimDocument();
                    document.FileName = model.File.FileName;
                    document.DateUploaded = DateTime.Now;
                    document.ClaimId = claimId;
                    documentService.AddClaimDocument(document);
                }
                return RedirectToAction("Index", "Lecturer");

            }
            ViewBag.Message = "Error saving claim details";

            return View(model);
        }

        public IActionResult Download(int claimId)
        {
            var documents = documentService.GetClaimDocuments(claimId);
            if (!documents.Any())
            {
                return Content("Filename is not provided.");
            }
            var document = documents.FirstOrDefault();
            string filePath = Path.Combine(_environment.WebRootPath, "uploads", document.FileName);
            if (!System.IO.File.Exists(filePath))
            {
                return Content("File not found.");
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", document.FileName);
        }
    }
}
