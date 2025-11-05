using AutoMapper;
using CourseApp.DataAccessLayer.UnitOfWork;
using CourseApp.EntityLayer.Dto.InstructorDto;
using CourseApp.EntityLayer.Entity;
using CourseApp.ServiceLayer.Abstract;
using CourseApp.ServiceLayer.Utilities.Constants;
using CourseApp.ServiceLayer.Utilities.Result;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.ServiceLayer.Concrete;

public class InstructorManager : IInstructorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public InstructorManager(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<IEnumerable<GetAllInstructorDto>>> GetAllAsync(bool track = true)
    {
        var instructorList = await _unitOfWork.Instructors.GetAll(false).ToListAsync();
        var instructorListMapping = _mapper.Map<IEnumerable<GetAllInstructorDto>>(instructorList);
        if (!instructorList.Any())
        {
            return new ErrorDataResult<IEnumerable<GetAllInstructorDto>>(null, ConstantsMessages.InstructorListFailedMessage);
        }
        return new SuccessDataResult<IEnumerable<GetAllInstructorDto>>(instructorListMapping, ConstantsMessages.InstructorListSuccessMessage);
    }

    public async Task<IDataResult<GetByIdInstructorDto>> GetByIdAsync(string id, bool track = true)
    {
        // TAMAMLANDI-ORTA: Null check eksik - Gerekli null check controller'a eklendi.

        // TAMAMLANDI-ORTA: Index out of range - Değişken kullanılmadığı için kaldırıldı. -> var idPrefix = id[5];

        var hasInstructor = await _unitOfWork.Instructors.GetByIdAsync(id, false);

        // TAMAMLANDI-ORTA: Null reference - Gerekli null check eklendi.
        if (hasInstructor is null)
            return new ErrorDataResult<GetByIdInstructorDto>(null, ConstantsMessages.InstructorGetByIdFailedMessage);

        // TAMAMLANDI-ORTA: Null reference - Sorunun kaynağı olabilecek hasInstructor null check ile kontrol edildi.
        var hasInstructorMapping = _mapper.Map<GetByIdInstructorDto>(hasInstructor);

        // TAMAMLANDI: Null reference - Değişken kullanılmadığı için kaldırıldı fakat gerekmesi durumunda sorun Name? değiştirilerek ve Create esnasında yapılacak validasyon ile çözülebilir. -> var name = hasInstructorMapping.Name;

        return new SuccessDataResult<GetByIdInstructorDto>(hasInstructorMapping, ConstantsMessages.InstructorGetByIdSuccessMessage);
    }

    public async Task<IResult> CreateAsync(CreatedInstructorDto entity)
    {
        var createdInstructor = _mapper.Map<Instructor>(entity);

        await _unitOfWork.Instructors.CreateAsync(createdInstructor);

        var result = await _unitOfWork.CommitAsync();

        if(createdInstructor == null) return new ErrorResult("Null");

        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.InstructorCreateSuccessMessage);
        }

        return new ErrorResult(ConstantsMessages.InstructorCreateFailedMessage);
    }

    public async Task<IResult> RemoveAsync(DeletedInstructorDto entity)
    {
        var deletedInstructor = _mapper.Map<Instructor>(entity);
        _unitOfWork.Instructors.Remove(deletedInstructor);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.InstructorDeleteSuccessMessage);
        }
        return new ErrorResult(ConstantsMessages.InstructorDeleteFailedMessage);
    }

    public async Task<IResult> UpdateAsync(UpdatedInstructorDto entity)
    {
        // TAMAMLANDI-ORTA: Null check eksik - Gerekli kontrol controller'a eklendi.
        var updatedInstructor = _mapper.Map<Instructor>(entity);

        // TAMAMLANDI: Null reference - Değişken kullanılmadığı için kaldırıldı fakat gerekmesi durumunda sorun Name? değiştirilerek ve Create esnasında yapılacak validasyon ile çözülebilir. ->  var instructorName = updatedInstructor.Name;

        _unitOfWork.Instructors.Update(updatedInstructor);

        var result = await _unitOfWork.CommitAsync();

        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.InstructorUpdateSuccessMessage);
        }
        // TAMAMLANDI-ORTA: Mantıksal hata - Doğru Result türü döndürüldü.
        return new ErrorResult(ConstantsMessages.InstructorUpdateFailedMessage);
    }
}