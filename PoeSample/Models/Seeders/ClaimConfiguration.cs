using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PoeSample.Models.Seeders
{

    public class ClaimConfiguration : IEntityTypeConfiguration<Claim>
    {
        public void Configure(EntityTypeBuilder<Claim> builder)
        {
            builder.HasKey(b => b.Id);
            builder.HasOne(p => p.Person).WithMany(x => x.Claims);
            builder.HasData
            (
                new Claim
                {
                    Id = 1,
                    StatusId = 2,
                    PersonId = 1,
                    DateClaimed = DateTime.Now,
                    Rate = 800,
                    ClassId = 1,
                    CourseId = 2,
                    Hours = 3,
                    TotalFee = 2400,

                },
                  new Claim
                  {
                      Id = 2,
                      StatusId = 2,
                      PersonId = 2,
                      DateClaimed = DateTime.Now,
                      Rate = 800,
                      ClassId = 1,
                      CourseId = 2,
                      Hours = 3,
                      TotalFee = 2400,

                  }
                );

        }
    }
}
