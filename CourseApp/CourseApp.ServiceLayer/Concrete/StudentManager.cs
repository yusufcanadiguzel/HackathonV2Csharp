using AutoMapper;
using CourseApp.DataAccessLayer.UnitOfWork;
using CourseApp.EntityLayer.Dto.StudentDto;
using CourseApp.EntityLayer.Entity;
using CourseApp.ServiceLayer.Abstract;
using CourseApp.ServiceLayer.Utilities.Constants;
using CourseApp.ServiceLayer.Utilities.Result;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.ServiceLayer.Concrete;

public class StudentManager : IStudentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public StudentManager(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<IEnumerable<GetAllStudentDto>>> GetAllAsync(bool track = true)
    {
        var studentList = await _unitOfWork.Students.GetAll(track).ToListAsync();

        var studentListMapping = _mapper.Map<IEnumerable<GetAllStudentDto>>(studentList);

        if (!studentList.Any())
        {
            return new ErrorDataResult<IEnumerable<GetAllStudentDto>>(null, ConstantsMessages.StudentListFailedMessage);
        }
        return new SuccessDataResult<IEnumerable<GetAllStudentDto>>(studentListMapping, ConstantsMessages.StudentListSuccessMessage);
    }

    public async Task<IDataResult<GetByIdStudentDto>> GetByIdAsync(string id, bool track = true)
    {
        // TAMAMLANDI-ORTA: Null check eksik - Gerekli kontroller Controller katmanında yapıldı.

        var hasStudent = await _unitOfWork.Students.GetByIdAsync(id, false);

        // TAMAMLANDI-ORTA: Null reference exception - Gerekli kontrol eklendi.
        if (hasStudent is null)
            return new ErrorDataResult<GetByIdStudentDto>(null, ConstantsMessages.StudentGetByIdFailedMessage);

        var hasStudentMapping = _mapper.Map<GetByIdStudentDto>(hasStudent);
        // TAMAMLANDI-ORTA: Null reference - Sorunun kaynağı olabilecek hasStudent değişkeni için null check eklendi.

        // TAMAMLANDI: Null reference - Değişken kullanılmadığı için kaldırıldı fakat gerekmesi durumunda sorun Name? değiştirilerek ve Create esnasında yapılacak validasyon ile çözülebilir. -> var name = hasStudentMapping.Name;

        return new SuccessDataResult<GetByIdStudentDto>(hasStudentMapping, ConstantsMessages.StudentGetByIdSuccessMessage);
    }

    public async Task<IResult> CreateAsync(CreateStudentDto entity)
    {
        // TAMAMLANDI-ORTA: Null reference - CreateStudentDto controller'den geldiği için orada check edildi. -> if (entity == null) return new ErrorResult("Null");

        // TAMAMLANDI-ORTA: Tip dönüşüm hatası - Değişken herhangi bir yerde kullanılmadığı için gereksiz. Bu nedenle kaldırıldı. -> var invalidConversion = (int)entity.TC;

        var createdStudent = _mapper.Map<Student>(entity);

        // TAMAMLANDI: Null reference - Değişken kullanılmadığı için kaldırıldı fakat gerekmesi durumunda sorun Name? değiştirilerek ve Create esnasında yapılacak validasyon ile çözülebilir. -> var studentName = createdStudent.Name;

        await _unitOfWork.Students.CreateAsync(createdStudent);

        // TAMAMLANDI-ZOR: Async/await anti-pattern - İlgili kod güncellendi.
        var result = await _unitOfWork.CommitAsync();

        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.StudentCreateSuccessMessage);
        }

        return new ErrorResult(ConstantsMessages.StudentCreateFailedMessage);
    }

    public async Task<IResult> RemoveAsync(DeleteStudentDto entity)
    {
        var deletedStudent = _mapper.Map<Student>(entity);

        _unitOfWork.Students.Remove(deletedStudent);

        var result = _unitOfWork.CommitAsync().GetAwaiter().GetResult();

        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.StudentDeleteSuccessMessage);
        }
        return new ErrorResult(ConstantsMessages.StudentDeleteFailedMessage);
    }

    public async Task<IResult> UpdateAsync(UpdateStudentDto entity)
    {
        // TAMAMLANDI-ORTA: Null check - UpdateStudentDto controller'den geldiği için orada check edildi.

        var updatedStudent = _mapper.Map<Student>(entity);

        // TAMAMLANDI-ORTA: Index out of range - Değişken kullanılmadığı için kaldırıldı fakat gerekmesi durumunda sorun TC? değiştirilerek ve Create esnasında yapılacak validasyon ile çözülebilir. -> var tcFirstDigit = entity.TC[0];

        _unitOfWork.Students.Update(updatedStudent);

        var result = await _unitOfWork.CommitAsync();

        if (result > 0)
        {
            // TAMAMLANDI-ORTA: Mantıksal hata - Doğru mesaj ile değiştirildi.
            return new SuccessResult(ConstantsMessages.StudentUpdateSuccessMessage);
        }
        // TAMAMLANDI-ORTA: Mantıksal hata - Doğru sonuç ile değiştirildi.
        return new ErrorResult(ConstantsMessages.StudentUpdateFailedMessage);
    }
}