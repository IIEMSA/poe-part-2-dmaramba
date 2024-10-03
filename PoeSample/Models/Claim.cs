namespace PoeSample.Models
{
    public class Claim
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public DateTime DateClaimed { get; set; }
        public int CourseId { get; set; }

        public int ClassId { get; set; }

    }
}
