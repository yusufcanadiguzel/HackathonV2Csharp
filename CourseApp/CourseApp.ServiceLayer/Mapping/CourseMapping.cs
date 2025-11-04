using AutoMapper;
using CourseApp.EntityLayer.Dto.CourseDto;
using CourseApp.EntityLayer.Entity;

namespace CourseApp.BusinessLayer.Mapping
{
    public class CourseMapping : Profile
    {
        public CourseMapping()
        {
            CreateMap<CreateCourseDto, Course>();
        }
    }
}