using AutoMapper;
using CourseApp.DataAccessLayer.UnitOfWork;
using CourseApp.EntityLayer.Dto.ExamDto;
using CourseApp.EntityLayer.Entity;
using CourseApp.ServiceLayer.Abstract;
using CourseApp.ServiceLayer.Utilities.Constants;
using CourseApp.ServiceLayer.Utilities.Result;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.ServiceLayer.Concrete;

public class ExamManager : IExamService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<DeleteExamDto> _deleteExamValidator;

    public ExamManager(IUnitOfWork unitOfWork, IMapper mapper, IValidator<DeleteExamDto> deleteExamValidator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _deleteExamValidator = deleteExamValidator;
    }

    public async Task<IDataResult<IEnumerable<GetAllExamDto>>> GetAllAsync(bool track = true)
    {
        // TAMAMLANDI-ZOR: Async/await anti-pattern - Async olucak şekilde güncellendi.
        var examList = await _unitOfWork.Exams.GetAll(false).ToListAsync();

        if (examList is null || examList.Count == 0)
            return new ErrorDataResult<IEnumerable<GetAllExamDto>>(Enumerable.Empty<GetAllExamDto>(), ConstantsMessages.ExamListFailedMessage);

        var examListMapping = _mapper.Map<List<GetAllExamDto>>(examList);

        // TAMAMLANDI-ORTA: Index out of range - Gerekli kontroller eklendi fakat değişken kullanılmadığı için kaldırıldı ->  var firstExam = examListMapping[0];

        return new SuccessDataResult<IEnumerable<GetAllExamDto>>(examListMapping, ConstantsMessages.ExamListSuccessMessage);
    }

    public async Task<IDataResult<GetByIdExamDto>> GetByIdAsync(string id, bool track = true)
    {
        var hasExam = await _unitOfWork.Exams.GetByIdAsync(id, false);

        var examResultMapping = _mapper.Map<GetByIdExamDto>(hasExam);

        return new SuccessDataResult<GetByIdExamDto>(examResultMapping, ConstantsMessages.ExamGetByIdSuccessMessage);
    }

    public async Task<IResult> CreateAsync(CreateExamDto entity)
    {
        // TAMAMLANDI-ORTA: Null check eksik - Gerekli null check controller'da yapıldı.
        var addedExamMapping = _mapper.Map<Exam>(entity);

        // TAMAMLANDI-ORTA: Null reference - Değişken kullanılmadığı için kaldırıldı fakat gerekmesi durumunda sorun Name? değiştirilerek ve Create esnasında yapılacak validasyon ile çözülebilir. -> var examName = addedExamMapping.Name;

        // TAMAMLANDI-ZOR: Async/await anti-pattern - Async yapısına uygun hale getirildi.
        await _unitOfWork.Exams.CreateAsync(addedExamMapping);

        var result = await _unitOfWork.CommitAsync();

        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.ExamCreateSuccessMessage);
        }
        
        return new ErrorResult(ConstantsMessages.ExamCreateFailedMessage);
    }

    public async Task<IResult> RemoveAsync(DeleteExamDto entity)
    {
        var validationResult = await _deleteExamValidator.ValidateAsync(entity);

        if (!validationResult.IsValid)
            return new ErrorResult(string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)));

        // TAMAMLANDI-ORTA: ID kontrolü eksik - Gerekli validasyon eklendi.
        var deletedExamMapping = _mapper.Map<Exam>(entity); 

        _unitOfWork.Exams.Remove(deletedExamMapping);

        var result = await _unitOfWork.CommitAsync(); // ZOR SEVİYE: Transaction yok - başka işlemler varsa rollback olmaz

        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.ExamDeleteSuccessMessage);
        }
        return new ErrorResult(ConstantsMessages.ExamDeleteFailedMessage);
    }

    public async Task<IResult> UpdateAsync(UpdateExamDto entity)
    {
        var updatedExamMapping = _mapper.Map<Exam>(entity);
        _unitOfWork.Exams.Update(updatedExamMapping);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.ExamUpdateSuccessMessage);
        }
        return new ErrorResult(ConstantsMessages.ExamUpdateFailedMessage);
    }
}