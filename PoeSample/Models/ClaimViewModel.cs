using Microsoft.AspNetCore.Mvc.Rendering;

namespace PoeSample.Models
{
    public class ClaimViewModel
    {
        public int CourseId { get; set; }
        public int ClassId { get; set; }
        public int Hours { get; set; }

        public IFormFile File { get; set; }
        public SelectList? Courses { get; set; }

        public SelectList? Classes { get; set; }


    }
}
