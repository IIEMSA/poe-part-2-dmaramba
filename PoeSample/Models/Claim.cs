namespace PoeSample.Models
{
    public class Claim
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public DateTime DateClaimed { get; set; }
        public int CourseId { get; set; }
        public double Rate {  get; set; }
        public int Hours { get; set; }

        //claculated field
        public double TotalFee { get; set; }

        public int PersonId { get; set; }
        public int ClassId { get; set; }

        //relationships
        public virtual Person Person { get; set; }

    }
}
