namespace PoeSample.Models
{
    public class ClaimItemModel
    {
        public int Id { get; set; }
        public DateTime DateClaimed { get; set; }
        public string PersonName { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string CourseName { get; set; }
        public string ClassName { get; set; }

        public int Hours { get; set; }
        public double Rate { get; set; }
        public double Total { get; set; }

    }
}
