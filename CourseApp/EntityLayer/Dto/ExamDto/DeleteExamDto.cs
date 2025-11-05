namespace CourseApp.EntityLayer.Dto.ExamDto;

public record DeleteExamDto
{
    public string Id { get; init; }
    public string? Name { get; init; }
    public DateTime Date { get; init; }
}