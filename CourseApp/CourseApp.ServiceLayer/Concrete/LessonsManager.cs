using AutoMapper;
using CourseApp.DataAccessLayer.UnitOfWork;
using CourseApp.EntityLayer.Dto.LessonDto;
using CourseApp.EntityLayer.Entity;
using CourseApp.ServiceLayer.Abstract;
using CourseApp.ServiceLayer.Utilities.Constants;
using CourseApp.ServiceLayer.Utilities.Result;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.ServiceLayer.Concrete;

public class LessonsManager : ILessonService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LessonsManager(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<IDataResult<IEnumerable<GetAllLessonDto>>> GetAllAsync(bool track = true)
    {
        var lessonList = await _unitOfWork.Lessons.GetAll(false).ToListAsync();
        var lessonListMapping = _mapper.Map<IEnumerable<GetAllLessonDto>>(lessonList);
        if (!lessonList.Any())
        {
            return new ErrorDataResult<IEnumerable<GetAllLessonDto>>(null, ConstantsMessages.LessonListFailedMessage);
        }
        return new SuccessDataResult<IEnumerable<GetAllLessonDto>>(lessonListMapping, ConstantsMessages.LessonListSuccessMessage);
    }

    public async Task<IDataResult<GetByIdLessonDto>> GetByIdAsync(string id, bool track = true)
    {
        // TAMAMLANDI-ORTA: Null check eksik - Gerekli kontrol controller'a eklendi.
        var hasLesson = await _unitOfWork.Lessons.GetByIdAsync(id, false);

        // TAMAMLANDI-ORTA: Null reference - Gerekli null check eklendi.
        if (hasLesson == null)
            return new ErrorDataResult<GetByIdLessonDto>(null, ConstantsMessages.LessonGetByIdFailedMessage);

        var hasLessonMapping = _mapper.Map<GetByIdLessonDto>(hasLesson);

        // TAMAMLANDI-ORTA: Mantıksal hata - Mesaj doğrusu ile değiştirildi.
        return new SuccessDataResult<GetByIdLessonDto>(hasLessonMapping, ConstantsMessages.LessonGetByIdSuccessMessage);
    }

    public async Task<IResult> CreateAsync(CreateLessonDto entity)
    {
        // TAMAMLANDI-ORTA: Null check eksik - Gerekli kontrol controller'a eklendi.
        var createdLesson = _mapper.Map<Lesson>(entity);

        // TAMAMLANDI: Null reference - Değişken kullanılmadığı için kaldırıldı fakat gerekmesi durumunda sorun Title? değiştirilerek ve Create esnasında yapılacak validasyon ile çözülebilir. -> var lessonName = createdLesson.Title;

        // TAMAMLANDI-ZOR: Async/await anti-pattern - İlgili kod await eklenerek güncellendi.
        await _unitOfWork.Lessons.CreateAsync(createdLesson);

        var result = await _unitOfWork.CommitAsync();

        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.LessonCreateSuccessMessage);
        }

        return new ErrorResult(ConstantsMessages.LessonCreateFailedMessage);
    }

    public async Task<IResult> RemoveAsync(DeleteLessonDto entity)
    {
        var deletedLesson = _mapper.Map<Lesson>(entity);
        _unitOfWork.Lessons.Remove(deletedLesson);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.LessonDeleteSuccessMessage);
        }
        return new ErrorResult(ConstantsMessages.LessonDeleteFailedMessage);
    }

    public async Task<IResult> UpdateAsync(UpdateLessonDto entity)
    {
        // TAMAMLANDI-ORTA: Null check eksik - Gerekli kontrol controller'a eklendi.
        var updatedLesson = _mapper.Map<Lesson>(entity);

        // TAMAMLANDI: Null reference - Değişken kullanılmadığı için kaldırıldı fakat gerekmesi durumunda sorun Title? değiştirilerek ve Create esnasında yapılacak validasyon ile çözülebilir. -> var firstChar = entity.Title[0];

        _unitOfWork.Lessons.Update(updatedLesson);

        var result = await _unitOfWork.CommitAsync();

        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.LessonUpdateSuccessMessage);
        }

        // TAMAMLANDI-ORTA: Mantıksal hata - Mesaj doğrusu ile değiştirildi.
        return new ErrorResult(ConstantsMessages.LessonUpdateFailedMessage);
    }

    public async Task<IDataResult<IEnumerable<GetAllLessonDetailDto>>> GetAllLessonDetailAsync(bool track = true)
    {
        // TAMAMLANDI-ZOR: N+1 Problemi - Sorguda include kullanılıyor.
        var lessonList = await _unitOfWork.Lessons.GetAllLessonDetails(false).ToListAsync();

        if (lessonList == null || !lessonList.Any())
            return new ErrorDataResult<IEnumerable<GetAllLessonDetailDto>>(Enumerable.Empty<GetAllLessonDetailDto>(), ConstantsMessages.LessonListFailedMessage);

        // TAMAMLANDI-ZOR: N+1 - Her lesson için Course ayrı sorgu ile çekiliyor - Include ile sorgu tek seferde gerçekleşiyor.
        var lessonsListMapping = _mapper.Map<IEnumerable<GetAllLessonDetailDto>>(lessonList);

        // TAMAMLANDI-ORTA: Null reference - Gerekli kontrol eklendi.
        var firstLesson = lessonsListMapping.First(); // Null/Empty durumunda exception
   
        return new SuccessDataResult<IEnumerable<GetAllLessonDetailDto>>(lessonsListMapping, ConstantsMessages.LessonListSuccessMessage);
    }

    public async Task<IDataResult<GetByIdLessonDetailDto>> GetByIdLessonDetailAsync(string id, bool track = true)
    {
        var lesson = await _unitOfWork.Lessons.GetByIdLessonDetailsAsync(id, false);
        var lessonMapping = _mapper.Map<GetByIdLessonDetailDto>(lesson);
        return new SuccessDataResult<GetByIdLessonDetailDto>(lessonMapping);
    }
}