using CourseApp.EntityLayer.Dto.RegistrationDto;
using CourseApp.ServiceLayer.Utilities.Result;

namespace CourseApp.ServiceLayer.Abstract;

public interface IRegistrationService
{
    Task<IDataResult<IEnumerable<GetAllRegistrationDto>>> GetAllAsync(bool track = true);
    Task<IDataResult<GetByIdRegistrationDto>> GetByIdAsync(string id, bool track = true);
    Task<IResult> CreateAsync(CreateRegistrationDto entity);
    Task<IResult> UpdateAsync(UpdatedRegistrationDto entity);
    Task<IResult> RemoveAsync(DeleteRegistrationDto entity);
    Task<IDataResult<IEnumerable<GetAllRegistrationDetailDto>>> GetAllRegistrationDetailAsync(bool track = true);
    Task<IDataResult<GetByIdRegistrationDetailDto>> GetByIdRegistrationDetailAsync(string id, bool track = true);
}
