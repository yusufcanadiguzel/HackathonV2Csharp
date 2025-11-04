using Autofac;
using CourseApp.BusinessLayer.Validation.FluentValidation.Exam;
using CourseApp.BusinessLayer.Validation.FluentValidation.ExamResult;
using CourseApp.BusinessLayer.Validation.FluentValidation.Instructor;
using CourseApp.BusinessLayer.Validation.FluentValidation.Lesson;
using CourseApp.BusinessLayer.Validation.FluentValidation.Registration;
using CourseApp.DataAccessLayer.Concrete;
using CourseApp.DataAccessLayer.UnitOfWork;
using CourseApp.EntityLayer.Dto.ExamDto;
using CourseApp.EntityLayer.Dto.ExamResultDto;
using CourseApp.EntityLayer.Dto.InstructorDto;
using CourseApp.EntityLayer.Dto.LessonDto;
using CourseApp.EntityLayer.Dto.RegistrationDto;
using CourseApp.ServiceLayer.Concrete;
using FluentValidation;

namespace CourseApp.BusinessLayer.DependencyResolvers.Autofac
{
    public class AutofacDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // UnitOfWork Configuration
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            // Dynamic Repository Registration
            var repositoryAssembly = System.Reflection.Assembly.GetAssembly(typeof(CourseRepository));

            builder.RegisterAssemblyTypes(repositoryAssembly!)
                .Where(x => x.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Dynamic Service Registration
            var serviceAssembly = System.Reflection.Assembly.GetAssembly(typeof(CourseManager));

            builder.RegisterAssemblyTypes(serviceAssembly!)
                .Where(x => x.Name.EndsWith("Manager"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Instructor Service Registration
            builder.RegisterType<CreatedInstructorDtoValidator>().As<IValidator<CreatedInstructorDto>>().InstancePerLifetimeScope();

            // Lesson Service Registration
            builder.RegisterType<CreateLessonDtoValidator>().As<IValidator<CreateLessonDto>>().InstancePerLifetimeScope();

            // Exam Service Registration
            builder.RegisterType<DeleteExamDtoValidator>().As<IValidator<DeleteExamDto>>().InstancePerLifetimeScope();

            // ExamResult Service Registration
            builder.RegisterType<CreateExamResultDtoValidator>().As<IValidator<CreateExamResultDto>>().InstancePerLifetimeScope();

            // Registration Service Registration
            builder.RegisterType<CreateRegistrationDtoValidator>().As<IValidator<CreateRegistrationDto>>().InstancePerLifetimeScope();
        }
    }
}