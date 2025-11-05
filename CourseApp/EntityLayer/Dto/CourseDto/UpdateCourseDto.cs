namespace CourseApp.EntityLayer.Dto.CourseDto;

public record UpdateCourseDto
{
    public string Id { get; init; } = null!;
    public bool IsActive { get; init; } = false;
    public DateTime CreatedDate { get; init; } = DateTime.Now;
    public string? CourseName { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string? InstructorID { get; init; }
}