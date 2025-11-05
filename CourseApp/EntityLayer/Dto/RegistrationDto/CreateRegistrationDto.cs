namespace CourseApp.EntityLayer.Dto.RegistrationDto;

public record CreateRegistrationDto
{
    public DateTime RegistrationDate { get; init; } = DateTime.Now;
    public decimal Price { get; init; }
    public string? StudentID { get; init; }
    public string? CourseID { get; init; }
}