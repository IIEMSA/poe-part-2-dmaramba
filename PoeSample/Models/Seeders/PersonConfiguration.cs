using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data;

namespace PoeSample.Models.Seeders
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(b => b.Id);
            builder.HasData
            (
                new Person
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    UserName = "john@123.com",
                    Password = "password1",
                    Role = "Lecturer"
                },
                  new Person
                  {
                      Id = 2,
                      FirstName = "Harry",
                      LastName = "Brodersen",
                      UserName = "hary@123.com",
                      Password = "password2",
                      Role = "Coordinator"
                  });
        }
    }
}
