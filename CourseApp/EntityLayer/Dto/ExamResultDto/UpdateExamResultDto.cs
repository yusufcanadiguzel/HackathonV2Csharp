namespace CourseApp.EntityLayer.Dto.ExamResultDto;

public record UpdateExamResultDto
{
    public string Id { get; init; } = null!;
    public byte Grade { get; init; }
    public string? ExamID { get; init; }
    public string? StudentID { get; init; }
}