using CourseApp.BusinessLayer.Validation.FluentValidation;
using CourseApp.BusinessLayer.Validation.FluentValidation.Instructor;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace CourseApp.API.Extensions
{
    public static class ServicesExtensions
    {
        // FluentValidation konfigürasyonu ve otomatik doğrulama eklenmesi
        public static void ConfigureValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CreatedInstructorDtoValidator>();
        }
    }
}
