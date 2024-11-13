using Microsoft.AspNetCore.Mvc;
using PoeSample.Models;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using PoeSample.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Reflection.PortableExecutable;
using static iTextSharp.text.pdf.AcroFields;

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
            HttpContext.Session.SetString("ClaimsData", JsonConvert.SerializeObject(claims));
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


        public IActionResult DownloadReport()
        {
            string fileName = DateTime.Now.ToFileTime() + ".pdf";
            //get data from session
            var claimsData = HttpContext.Session.GetString("ClaimsData");
            var claims = JsonConvert.DeserializeObject<List<ClaimItemModel>>(claimsData!);
            iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 30, 30, 30, 40);
            string uploadPath = Path.Combine(_environment.WebRootPath, "uploads");
            var fullPath = Path.Combine(uploadPath, fileName);
            var fs = new FileStream(fullPath, FileMode.Create);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            Font header = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK);
            Font data = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK);
            document.Open();
            document.Add(new Paragraph("Claims Report", header));
            document.Add(new Paragraph($"Date {DateTime.Now.ToString("dd MMM yyyy HH:mm")}", header));

            PdfPTable table = new PdfPTable(7);
            table.HorizontalAlignment = 0;
            table.WidthPercentage = 100;
            float[] widths = new float[] { 30, 20, 10, 10, 10, 10, 10 };
            table.SetWidths(widths);
            PdfPCell cell = new PdfPCell();
            cell.Phrase = new Phrase("Course", header);
            table.AddCell(cell);

            cell = new PdfPCell();
            cell.Phrase = new Phrase("Class", header);
            table.AddCell(cell);

            cell = new PdfPCell();
            cell.Phrase = new Phrase("Date", header);
            table.AddCell(cell);

            cell = new PdfPCell();
            cell.Phrase = new Phrase("Hours", header);
            table.AddCell(cell);

            cell = new PdfPCell();
            cell.Phrase = new Phrase("Rate", header);
            table.AddCell(cell);

            cell = new PdfPCell();
            cell.Phrase = new Phrase("Total", header);
            table.AddCell(cell);

            cell = new PdfPCell();
            cell.Phrase = new Phrase("Status", header);
            table.AddCell(cell);


            foreach (var item in claims!)
            {

                cell = new PdfPCell();
                cell.Phrase = new Phrase(item.CourseName, data);
                table.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(item.ClassName, data);
                table.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(item.DateClaimed.ToString("dd MMM yyyy"), data);
                table.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(item.Hours.ToString(), data);
                table.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(item.Rate.ToString(), data);
                table.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(item.Total.ToString(), data);
                table.AddCell(cell);

                cell = new PdfPCell();
                cell.Phrase = new Phrase(item.Status, data);
                table.AddCell(cell);

            

            }


            cell = new PdfPCell();
            cell.Colspan = 5;
            cell.Phrase = new Phrase("TOTAL", header);
            table.AddCell(cell);



            cell = new PdfPCell();
            cell.Colspan = 2;
            cell.Phrase = new Phrase(claims.Sum(x=>x.Total).ToString(), header);
            table.AddCell(cell);


        

            document.Add(new Paragraph(Environment.NewLine));
            document.Add(table);

            document.Close();
            writer.Close();
            fs.Close();

            return Redirect($"/uploads/{fileName}");
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
