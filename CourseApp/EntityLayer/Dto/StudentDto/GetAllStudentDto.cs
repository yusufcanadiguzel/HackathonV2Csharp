namespace CourseApp.EntityLayer.Dto.StudentDto;

public record GetAllStudentDto
{
    public string Id { get; init; }
    public string? Name { get; init; }
    public string? Surname { get; init; }
    public string? Fullname => $"{Name} {Surname}";
    public DateTime BirthDate { get; init; }
    public string? TC { get; init; }
}