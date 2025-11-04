using CourseApp.EntityLayer.Dto.RegistrationDto;
using CourseApp.ServiceLayer.Utilities.Constants;
using FluentValidation;

namespace CourseApp.BusinessLayer.Validation.FluentValidation.Registration
{
    public class CreateRegistrationDtoValidator : AbstractValidator<CreateRegistrationDto>
    {
        public CreateRegistrationDtoValidator()
        {
            RuleFor(r => r.Price).NotEmpty().NotNull().WithMessage(ConstantsMessages.RegistrationPriceNotEmptyValidationMessage);
        }
    }
}
