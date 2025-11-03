using CourseApp.BusinessLayer.Validation.FluentValidation;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace CourseApp.API.Extensions
{
    public static class ServicesExtensions
    {
        // FluentValidation konfigurasyonu ve otomatik doğrulama eklenmesi
        public static void ConfigureValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<FvValidatorReference>();
            services.AddFluentValidationAutoValidation();
        }
    }
}
