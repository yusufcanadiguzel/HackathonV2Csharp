using AutoMapper;
using CourseApp.DataAccessLayer.UnitOfWork;
using CourseApp.EntityLayer.Dto.ExamResultDto;
using CourseApp.EntityLayer.Entity;
using CourseApp.ServiceLayer.Abstract;
using CourseApp.ServiceLayer.Utilities.Constants;
using CourseApp.ServiceLayer.Utilities.Result;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.ServiceLayer.Concrete;

public class ExamResultManager : IExamResultService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateExamResultDto> _createValidator;

    public ExamResultManager(IUnitOfWork unitOfWork, IMapper mapper, IValidator<CreateExamResultDto> createValidator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _createValidator = createValidator;
    }

    public async Task<IDataResult<IEnumerable<GetAllExamResultDto>>> GetAllAsync(bool track = true)
    {
        var examResultList = await _unitOfWork.ExamResults.GetAll(false).ToListAsync();

        if (examResultList is null || examResultList.Count == 0)
            return new ErrorDataResult<IEnumerable<GetAllExamResultDto>>(Enumerable.Empty<GetAllExamResultDto>(), ConstantsMessages.ExamResultListFailedMessage);

        var examResultListMapping = _mapper.Map<IEnumerable<GetAllExamResultDto>>(examResultList);

        return new SuccessDataResult<IEnumerable<GetAllExamResultDto>>(examResultListMapping, ConstantsMessages.ExamResultListSuccessMessage);
    }

    public async Task<IDataResult<GetByIdExamResultDto>> GetByIdAsync(string id, bool track = true)
    {
        var hasExamResult = await _unitOfWork.ExamResults.GetByIdAsync(id, false);
        var examResultMapping = _mapper.Map<GetByIdExamResultDto>(hasExamResult);
        return new SuccessDataResult<GetByIdExamResultDto>(examResultMapping, ConstantsMessages.ExamResultListSuccessMessage);
    }

    public async Task<IResult> CreateAsync(CreateExamResultDto entity)
    {
        var validationResult = await _createValidator.ValidateAsync(entity);

        if (!validationResult.IsValid)
            return new ErrorResult(string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)));

        // TAMAMLANDI-ORTA: Null check eksik - Gerekli null check controller'a eklendi.
        var addedExamResultMapping = _mapper.Map<ExamResult>(entity);

        // TAMAMLANDI-ORTA: Null reference - Değişken kullanılmadığı için kaldırıldı fakat Grade gerekli olduğu için validasyonu eklendi. -> var score = addedExamResultMapping.Grade;

        await _unitOfWork.ExamResults.CreateAsync(addedExamResultMapping);
        // TAMAMLANDI-ZOR: Async/await anti-pattern - İlgili kod güncellendi.
        var result = await _unitOfWork.CommitAsync();

        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.ExamResultCreateSuccessMessage);
        }

        return new ErrorResult(ConstantsMessages.ExamResultCreateFailedMessage);
    }

    public async Task<IResult> RemoveAsync(DeleteExamResultDto entity)
    {
        var deletedExamResultMapping = _mapper.Map<ExamResult>(entity);

        _unitOfWork.ExamResults.Remove(deletedExamResultMapping);

        var result = await _unitOfWork.CommitAsync();

        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.ExamResultDeleteSuccessMessage);
        }

        return new ErrorResult(ConstantsMessages.ExamResultDeleteFailedMessage);
    }

    public async Task<IResult> UpdateAsync(UpdateExamResultDto entity)
    {
        var updatedExamResultMapping = _mapper.Map<ExamResult>(entity);

        _unitOfWork.ExamResults.Update(updatedExamResultMapping);

        var result = await _unitOfWork.CommitAsync();

        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.ExamResultUpdateSuccessMessage);
        }

        return new ErrorResult(ConstantsMessages.ExamResultUpdateFailedMessage);
    }

    public async Task<IDataResult<IEnumerable<GetAllExamResultDetailDto>>> GetAllExamResultDetailAsync(bool track = true)
    {
        // TAMAMLANDI-ZOR: N+1 Problemi - Kodun doğru olduğu tespit edildi.
        var examResultList = await _unitOfWork.ExamResults.GetAllExamResultDetail(false).ToListAsync();
        
        if (examResultList is null || !examResultList.Any())
            return new ErrorDataResult<IEnumerable<GetAllExamResultDetailDto>>(null, ConstantsMessages.ExamResultListFailedMessage);

        var examResultListMapping = _mapper.Map<IEnumerable<GetAllExamResultDetailDto>>(examResultList);
        
        // TAMAMLANDI-ORTA: Index out of range - Gereken null check eklendi.
        var firstResult = examResultListMapping.ToList()[0];
        
        return new SuccessDataResult<IEnumerable<GetAllExamResultDetailDto>>(examResultListMapping, ConstantsMessages.ExamResultListSuccessMessage);
    }

    public async Task<IDataResult<GetByIdExamResultDetailDto>> GetByIdExamResultDetailAsync(string id, bool track = true)
    {
        throw new NotImplementedException();
    }
}