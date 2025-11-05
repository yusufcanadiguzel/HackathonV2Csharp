using CourseApp.EntityLayer.Entity;

namespace CourseApp.EntityLayer.Dto.CourseDto;

public record CreateCourseDto
{
    public bool IsActive { get; init; } = false;
    public DateTime CreatedDate { get; init; } = DateTime.Now;
    public string? CourseName { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string? InstructorID { get; init; }
    public string? InstructorName { get; init; }
}