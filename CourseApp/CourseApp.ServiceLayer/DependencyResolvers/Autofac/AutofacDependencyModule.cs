using Autofac;
using CourseApp.BusinessLayer.Validation.FluentValidation.Instructor;
using CourseApp.BusinessLayer.Validation.FluentValidation.Lesson;
using CourseApp.EntityLayer.Dto.InstructorDto;
using CourseApp.EntityLayer.Dto.LessonDto;
using FluentValidation;

namespace CourseApp.BusinessLayer.DependencyResolvers.Autofac
{
    public class AutofacDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Instructor Service Registration
            builder.RegisterType<CreatedInstructorDtoValidator>().As<IValidator<CreatedInstructorDto>>().InstancePerLifetimeScope();

            // Lesson Service Registration
            builder.RegisterType<CreateLessonDtoValidator>().As<IValidator<CreateLessonDto>>().InstancePerLifetimeScope();
        }
    }
}