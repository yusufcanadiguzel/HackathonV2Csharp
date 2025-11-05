using CourseApp.EntityLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseApp.DataAccessLayer.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasOne(c => c.Instructor).WithMany().HasForeignKey(c => c.InstructorID);
        }
    }
}