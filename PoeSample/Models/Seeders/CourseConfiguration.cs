using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PoeSample.Models.Seeders
{

    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(b => b.Id);
            builder.HasData
            (
                new Course
                {
                    Id = 1,
                    Code = "PROG6212",
                    Title = "Programming 2B"
                },
                  new Course
                  {
                      Id = 2,
                      Code = "AD100",
                      Title = "Advanced Databases"
                  }
                );

        }
    }

}