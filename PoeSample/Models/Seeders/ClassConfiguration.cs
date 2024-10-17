using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PoeSample.Models.Seeders
{

    public class ClassConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.HasKey(b => b.Id);
            builder.HasData
            (
                new Class
                {
                    Id = 1,
                    ClassName = "Group 1"
                },
                  new Class
                  {
                      Id = 2,
                      ClassName = "Group 2"
                  },
                  new Class
                  {
                      Id = 3,
                      ClassName = "Group 3"
                  }
                );

        }
    }

}