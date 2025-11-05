namespace CourseApp.EntityLayer.Dto.ExamDto;

public record GetAllExamDto
{
    public string Id { get; init; }
    public string? Name { get; init; }
    public DateTime Date { get; init; }
}