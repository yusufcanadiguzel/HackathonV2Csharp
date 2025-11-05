namespace CourseApp.EntityLayer.Dto.ExamResultDto;

public record GetAllExamResultDetailDto
{
    public string Id { get; init; } = null!;
    public byte Grade { get; init; }
    public string StudentName { get; init; } = null!;
    public string StudentSurname { get; init; } = null!;
    public string StudentFullName => $"{StudentName} {StudentSurname}";
    public string ExamName { get; init; } = null!;
    public string? ExamID { get; init; }
    public string? StudentID { get; init; }
}