using CourseApp.EntityLayer.Dto.LessonDto;
using CourseApp.ServiceLayer.Utilities.Constants;
using FluentValidation;

namespace CourseApp.BusinessLayer.Validation.FluentValidation.Lesson
{
    public class CreateLessonDtoValidator : AbstractValidator<CreateLessonDto>
    {
        public CreateLessonDtoValidator()
        {
            RuleFor(x => x.Title).Must(title => !string.IsNullOrWhiteSpace(title)).WithMessage(ConstantsMessages.LessonTitleNotEmptyValidationMessage);
        }
    }
}