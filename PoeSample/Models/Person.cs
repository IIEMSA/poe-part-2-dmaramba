namespace PoeSample.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //link to claims
        public virtual ICollection<Claim> Claims { get; set; }
    }
}
