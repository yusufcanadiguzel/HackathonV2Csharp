namespace CourseApp.EntityLayer.Dto.LessonDto;

public record GetByIdLessonDto
{
    public string Id { get; init; }
    public string? Title { get; init; }
    public DateTime Date { get; init; }
    public byte Duration { get; init; }
    public string? Content { get; init; }
    public string? CourseID { get; init; }
    public string? Time { get; init; }
}