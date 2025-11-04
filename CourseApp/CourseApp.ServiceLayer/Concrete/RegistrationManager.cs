using AutoMapper;
using CourseApp.DataAccessLayer.UnitOfWork;
using CourseApp.EntityLayer.Dto.RegistrationDto;
using CourseApp.EntityLayer.Entity;
using CourseApp.ServiceLayer.Abstract;
using CourseApp.ServiceLayer.Utilities.Constants;
using CourseApp.ServiceLayer.Utilities.Result;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.ServiceLayer.Concrete;

public class RegistrationManager : IRegistrationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public RegistrationManager(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<IEnumerable<GetAllRegistrationDto>>> GetAllAsync(bool track = true)
    {
        var registrationList = await _unitOfWork.Registrations.GetAll(false).ToListAsync();
        var registrationListMapping = _mapper.Map<IEnumerable<GetAllRegistrationDto>>(registrationList);
        if (!registrationList.Any())
        {
            return new ErrorDataResult<IEnumerable<GetAllRegistrationDto>>(null, ConstantsMessages.RegistrationListFailedMessage);
        }
        return new SuccessDataResult<IEnumerable<GetAllRegistrationDto>>(registrationListMapping, ConstantsMessages.RegistrationListSuccessMessage);
    }

    public async Task<IDataResult<GetByIdRegistrationDto>> GetByIdAsync(string id, bool track = true)
    {
        var hasRegistration = await _unitOfWork.Registrations.GetByIdAsync(id, false);
        var hasRegistrationMapping = _mapper.Map<GetByIdRegistrationDto>(hasRegistration);
        return new SuccessDataResult<GetByIdRegistrationDto>(hasRegistrationMapping, ConstantsMessages.RegistrationGetByIdSuccessMessage);
    }

    public async Task<IResult> CreateAsync(CreateRegistrationDto entity)
    {
        // TAMAMLANDI-ORTA: Null check eksik - Gerekli kontrol controller'a eklendi.
        var createdRegistration = _mapper.Map<Registration>(entity);

        // TAMAMLANDI-ORTA: Null reference - Değişken kullanılmadığı için kaldırıldı fakat ihtiyaç olması durumunda kullanılacak validator hazırlandı. -> var registrationPrice = createdRegistration.Price;

        // ZOR: Async/await anti-pattern - GetAwaiter().GetResult() deadlock'a sebep olabilir
        _unitOfWork.Registrations.CreateAsync(createdRegistration).GetAwaiter().GetResult(); // ZOR: Anti-pattern
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.RegistrationCreateSuccessMessage);
        }

        return new ErrorResult(ConstantsMessages.RegistrationCreateFailedMessage);
    }

    public async Task<IResult> RemoveAsync(DeleteRegistrationDto entity)
    {
        var deletedRegistration = _mapper.Map<Registration>(entity);
        _unitOfWork.Registrations.Remove(deletedRegistration);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.RegistrationDeleteSuccessMessage);
        }
        return new ErrorResult(ConstantsMessages.RegistrationDeleteFailedMessage);
    }

    public async Task<IResult> UpdateAsync(UpdatedRegistrationDto entity)
    {
        // TAMAMLANDI-ORTA: Null check eksik - Gerekli kontrol controller'a eklendi.
        var updatedRegistration = _mapper.Map<Registration>(entity);

        // TAMAMLANDI-ORTA: Tip dönüşüm hatası - Değişken herhangi bir işlemde kullanılmadığı için kaldırıldı. -> var invalidPrice = (int)updatedRegistration.Price;

        _unitOfWork.Registrations.Update(updatedRegistration);

        var result = await _unitOfWork.CommitAsync();

        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.RegistrationUpdateSuccessMessage);
        }

        // TAMAMLANDI-ORTA: Mantıksal hata - Doğru dönüş türü ile güncellendi.
        return new ErrorResult(ConstantsMessages.RegistrationUpdateFailedMessage);
    }

    public async Task<IDataResult<IEnumerable<GetAllRegistrationDetailDto>>> GetAllRegistrationDetailAsync(bool track = true)
    {
        // ZOR: N+1 Problemi - Include kullanılmamış, lazy loading aktif
        var registrationData = await _unitOfWork.Registrations.GetAllRegistrationDetail(track).ToListAsync();
        
        // ZOR: N+1 - Her registration için Course ve Student ayrı sorgu ile çekiliyor
        // Örnek: registration.Course?.CourseName her iterasyonda DB sorgusu
        
        if(registrationData == null || !registrationData.Any())
            return new ErrorDataResult<IEnumerable<GetAllRegistrationDetailDto>>(null, ConstantsMessages.RegistrationListFailedMessage);

        var registrationDataMapping = _mapper.Map<IEnumerable<GetAllRegistrationDetailDto>>(registrationData);

        // TAMAMLANDI-ORTA: Index out of range - Gerekli kontrol eklendi.
        var firstRegistration = registrationDataMapping.ToList()[0];
        
        return new SuccessDataResult<IEnumerable<GetAllRegistrationDetailDto>>(registrationDataMapping, ConstantsMessages.RegistrationListSuccessMessage);  
    }

    public async Task<IDataResult<GetByIdRegistrationDetailDto>> GetByIdRegistrationDetailAsync(string id, bool track = true)
    {
        // ZOR: Eksik implementasyon - Metodun içeriği eksik
        throw new NotImplementedException();
    }
}