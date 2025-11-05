using AutoMapper;
using CourseApp.DataAccessLayer.UnitOfWork;
using CourseApp.EntityLayer.Dto.CourseDto;
using CourseApp.EntityLayer.Entity;
using CourseApp.ServiceLayer.Abstract;
using CourseApp.ServiceLayer.Utilities.Constants;
using CourseApp.ServiceLayer.Utilities.Result;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.ServiceLayer.Concrete;

public class CourseManager : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CourseManager(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<IEnumerable<GetAllCourseDto>>> GetAllAsync(bool track = true)
    {
        // TAMAMLANDI:ZOR: N+1 Problemi - Repository'de sorgu sadeleştirilerek tek seferde verinin çekilmesi sağlandı.
        var courseList = await _unitOfWork.Courses.GetAll(false).ToListAsync();

        // TAMAMLANDI: Gerekli kontrol eklendi.
        if (courseList is null || !courseList.Any())
            return new ErrorDataResult<IEnumerable<GetAllCourseDto>>(Enumerable.Empty<GetAllCourseDto>(), ConstantsMessages.CourseListFailedMessage);

        var result = _mapper.Map<IEnumerable<GetAllCourseDto>>(courseList);

        /* TAMAMLANDI:ZOR: N+1 - Include/ThenInclude kullanılmamış - Repository'deki sorgu güncellendiği için böyle bir select koduna gerek kalmamıştır. ->
        var result = courseList.Select(course => new GetAllCourseDto
        {
            CourseName = course.CourseName,
            CreatedDate = course.CreatedDate,
            EndDate = course.EndDate,
            Id = course.ID,
            InstructorID = course.InstructorID,
            // TAMAMLANDI-ZOR: Her course için ayrı sorgu - course.Instructor?.Name çekiliyor
            IsActive = course.IsActive,
            StartDate = course.StartDate
        }).ToList();
        */

        // TAMAMLANDI: Dead code olduğu için kaldırıldı. -> var firstCourse = result[0];

        return new SuccessDataResult<IEnumerable<GetAllCourseDto>>(result, ConstantsMessages.CourseListSuccessMessage);
    }

    public async Task<IDataResult<GetByIdCourseDto>> GetByIdAsync(string id, bool track = true)
    {
        var hasCourse = await _unitOfWork.Courses.GetByIdAsync(id, track);

        if (hasCourse is null)
            return new ErrorDataResult<GetByIdCourseDto>(null, ConstantsMessages.CourseGetByIdFailedMessage);

        var course = _mapper.Map<GetByIdCourseDto>(hasCourse);

        return new SuccessDataResult<GetByIdCourseDto>(course, ConstantsMessages.CourseGetByIdSuccessMessage);
    }
    public async Task<IResult> CreateAsync(CreateCourseDto entity)
    {
        var course = _mapper.Map<Course>(entity);

        await _unitOfWork.Courses.CreateAsync(course);

        var result = await _unitOfWork.CommitAsync();

        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.CourseCreateSuccessMessage);
        }

        return new ErrorResult(ConstantsMessages.CourseCreateFailedMessage);
    }
    public async Task<IResult> Remove(DeleteCourseDto entity)
    {
        _unitOfWork.Courses.Remove(_mapper.Map<Course>(entity));

        var result = await _unitOfWork.CommitAsync();

        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.CourseDeleteSuccessMessage);
        }

        return new ErrorResult(ConstantsMessages.CourseDeleteFailedMessage);
    }

    public async Task<IResult> Update(UpdateCourseDto entity)
    {
        var updatedCourse = await _unitOfWork.Courses.GetByIdAsync(entity.Id);

        if (updatedCourse == null)
            return new ErrorResult(ConstantsMessages.CourseUpdateFailedMessage);

        _mapper.Map(entity, updatedCourse);

        _unitOfWork.Courses.Update(updatedCourse);

        var result = await _unitOfWork.CommitAsync();

        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.CourseUpdateSuccessMessage);
        }

        return new ErrorResult(ConstantsMessages.CourseUpdateFailedMessage);
    }

    public async Task<IDataResult<IEnumerable<GetAllCourseDetailDto>>> GetAllCourseDetail(bool track = true)
    {
        // TAMAMLANDI-ZOR: N+1 Problemi - Include kullanılmamış, lazy loading aktif - Repository sorgusu güncellendi.
        var courseListDetailList = await _unitOfWork.Courses.GetAllCourseDetail(false).ToListAsync();

        if (courseListDetailList is null)
            throw new NullReferenceException(ConstantsMessages.CourseGetByIdFailedMessage);

        var result = _mapper.Map<IEnumerable<GetAllCourseDetailDto>>(courseListDetailList);

        // DEAD CODE //
        //// TAMAMLANDI-ZOR: N+1 - Her course için Instructor ayrı sorgu ile çekiliyor (x.Instructor?.Name) - İlgili sorgulama güncellendi.
        //var courseDetailDtoList  = courseListDetailList.Select(x => new GetAllCourseDetailDto
        //{
        //    CourseName = x.CourseName,
        //    StartDate = x.StartDate,
        //    EndDate = x.EndDate,
        //    CreatedDate = x.CreatedDate,
        //    Id = x.ID,
        //    InstructorID = x.InstructorID,
        //    // TAMAMLANDI-ZOR: N+1 - Her course için ayrı Instructor sorgusu - İlgili sorgulama güncellendi.
        //    InstructorName = x.Instructor?.Name ?? "", // Lazy loading aktif - her iterasyonda DB sorgusu
        //    IsActive = x.IsActive,
        //});

        //var firstDetail = courseDetailDtoList.First();

        return new SuccessDataResult<IEnumerable<GetAllCourseDetailDto>>(result, ConstantsMessages.CourseDetailsFetchedSuccessfully);
    }

    private IResult CourseNameIsNullOrEmpty(string courseName)
    {
        if(courseName == null || courseName.Length == 0)
        {
            return new ErrorResult("Kurs Adı Boş Olamaz");
        }
        return new SuccessResult();
    }

    private async Task<IResult> CourseNameUniqeCheck(string id,string courseName)
    {
        var courseNameCheck = await _unitOfWork.Courses.GetAll(false).AnyAsync(c => c.CourseName == courseName);
        if(!courseNameCheck)
        {
            return new ErrorResult("Bu kurs adi ile zaten bir kurs var");
        }
        return new SuccessResult();
    }

    private  IResult CourseNameLenghtCehck(string courseName)
    {
        if(courseName == null || courseName.Length < 2 || courseName.Length > 50)
        {
            return new ErrorResult("Kurs Adı Uzunluğu 2 - 50 Karakter Arasında Olmalı");
        }
        return new SuccessResult();
    }

    private IResult IsValidDateFormat(string date)
    {
        DateTime tempDate;
        bool isValid = DateTime.TryParse(date, out tempDate);

        if (!isValid)
        {
            return new ErrorResult("Geçersiz tarih formatı.");
        }
        return new SuccessResult();
    }
    private IResult CheckCourseDates(DateTime startDate, DateTime endDate)
    {
        if (endDate <= startDate)
        {
            return new ErrorResult("Bitiş tarihi, başlangıç tarihinden sonra olmalıdır.");
        }
        return new SuccessResult();
    }
    
    private IResult CheckInstructorNameIsNullOrEmpty(string instructorName)
    {
        if (string.IsNullOrEmpty(instructorName))
        {
            return new ErrorResult("Eğitmen alanı boş olamaz");
        }

        return new SuccessResult();
    }
}
