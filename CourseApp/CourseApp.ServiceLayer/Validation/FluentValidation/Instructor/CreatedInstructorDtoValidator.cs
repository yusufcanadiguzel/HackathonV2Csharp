using CourseApp.EntityLayer.Dto.InstructorDto;
using CourseApp.ServiceLayer.Utilities.Constants;
using FluentValidation;

namespace CourseApp.BusinessLayer.Validation.FluentValidation.Instructor
{
    public class CreatedInstructorDtoValidator : AbstractValidator<CreatedInstructorDto>
    {
        public CreatedInstructorDtoValidator()
        {
            //RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage(ConstantsMessages.InstructorNameNotEmptyValidationMessage);
            RuleFor(x => x.Name).Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage(ConstantsMessages.InstructorNameNotEmptyValidationMessage);
        }
    }
}
