namespace PoeSample.Models
{
    public class ClaimDocument
    {
        public int Id { get; set; }
        public int ClaimId { get; set; }
        public DateTime DateUploaded { get; set; }
        public string FileName { get; set; }
    }
}
