using CourseApp.EntityLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.DataAccessLayer.Concrete;

public sealed class AppDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Registration> Registrations { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<ExamResult> ExamResults { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Konfigürasyonları otomatik olarak uygular.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}