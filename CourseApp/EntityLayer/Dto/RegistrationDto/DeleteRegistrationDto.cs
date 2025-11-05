namespace CourseApp.EntityLayer.Dto.RegistrationDto;

public record DeleteRegistrationDto
{
    public string Id { get; init; }
    public DateTime RegistrationDate { get; init; } = DateTime.Now;
    public decimal Price { get; init; }
    public string? StudentID { get; init; }
    public string? CourseID { get; init; }
}