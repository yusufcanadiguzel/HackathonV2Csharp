using CourseApp.EntityLayer.Dto.ExamResultDto;
using CourseApp.ServiceLayer.Utilities.Constants;
using FluentValidation;

namespace CourseApp.BusinessLayer.Validation.FluentValidation.ExamResult
{
    public class CreateExamResultDtoValidator : AbstractValidator<CreateExamResultDto>
    {
        public CreateExamResultDtoValidator()
        {
            RuleFor(e => e.Grade).NotNull().NotEmpty().WithMessage(ConstantsMessages.ExamResultGradeNotEmptyValidationMessage);
        }
    }
}
