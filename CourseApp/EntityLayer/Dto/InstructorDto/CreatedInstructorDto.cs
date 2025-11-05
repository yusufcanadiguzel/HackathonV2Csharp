namespace CourseApp.EntityLayer.Dto.InstructorDto;

public record CreatedInstructorDto
{
    public string? Name { get; init; }
    public string? Surname { get; init; }
    public string FullName => $"{Name} {Surname}";
    public string? Email { get; init; }
    public string? Professions { get; init; }
    public string? PhoneNumber { get; init; }
}