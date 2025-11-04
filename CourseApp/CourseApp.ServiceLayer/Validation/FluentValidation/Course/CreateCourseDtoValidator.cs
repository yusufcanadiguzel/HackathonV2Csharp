using CourseApp.EntityLayer.Dto.CourseDto;
using CourseApp.ServiceLayer.Utilities.Constants;
using FluentValidation;

namespace CourseApp.BusinessLayer.Validation.FluentValidation.Course
{
    public class CreateCourseDtoValidator : AbstractValidator<CreateCourseDto>
    {
        public CreateCourseDtoValidator()
        {
            RuleFor(x => x.CourseName).Must(courseName => !string.IsNullOrWhiteSpace(courseName)).WithMessage(ConstantsMessages.CourseNameNotNullMessage);
        }
    }
}
