namespace CourseApp.EntityLayer.Dto.RegistrationDto;

public record GetByIdRegistrationDetailDto
{
    public string Id { get; init; } = null!;
    public DateTime RegistrationDate { get; init; } = DateTime.Now;
    public decimal Price { get; init; }
    public string? StudentID { get; init; }
    public string? CourseID { get; init; }
    public string CourseName { get; init; } = null!;
    public string StudentName { get; init; } = null!;
    public string RegistrationDetail => $"{CourseName} {Price}";
}