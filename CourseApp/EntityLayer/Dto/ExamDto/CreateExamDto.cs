namespace CourseApp.EntityLayer.Dto.ExamDto;

public record CreateExamDto
{
    public string? Name { get; init; }
    public DateTime Date { get; init; }
}