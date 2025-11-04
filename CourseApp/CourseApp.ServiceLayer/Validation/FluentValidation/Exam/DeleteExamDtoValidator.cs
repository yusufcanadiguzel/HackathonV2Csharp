using CourseApp.EntityLayer.Dto.ExamDto;
using CourseApp.ServiceLayer.Utilities.Constants;
using FluentValidation;

namespace CourseApp.BusinessLayer.Validation.FluentValidation.Exam
{
    public class DeleteExamDtoValidator : AbstractValidator<DeleteExamDto>
    {
        public DeleteExamDtoValidator()
        {
            RuleFor(e => e.Id).Must(id => !string.IsNullOrWhiteSpace(id)).WithMessage(ConstantsMessages.ExamIdNotEmptyValidationMessage);
        }
    }
}
