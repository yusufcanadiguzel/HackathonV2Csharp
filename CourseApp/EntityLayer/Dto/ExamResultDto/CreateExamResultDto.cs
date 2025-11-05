namespace CourseApp.EntityLayer.Dto.ExamResultDto;

public record CreateExamResultDto
{
    public byte Grade { get; init; }
    public string? ExamID { get; init; }
    public string? StudentID { get; init; }
}