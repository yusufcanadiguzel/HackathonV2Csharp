namespace CourseApp.EntityLayer.Dto.ExamResultDto;

public record GetByIdExamResultDto
{
    public string Id { get; init; } = null!;
    public byte Grade { get; init; }
    public string Name { get; init; }   
    public string Surname { get; init; }
    public string? ExamID { get; init; }
    public string? StudentID { get; init; }
}