using CourseApp.EntityLayer.Dto.RegistrationDto;
using CourseApp.ServiceLayer.Abstract;
using CourseApp.ServiceLayer.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;

namespace CourseApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistrationsController : ControllerBase
{
    private readonly IRegistrationService _registrationService;

    public RegistrationsController(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _registrationService.GetAllAsync();
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _registrationService.GetByIdAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("detail")]
    public async Task<IActionResult> GetAllDetail()
    {
        var result = await _registrationService.GetAllRegistrationDetailAsync();
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("detail/{id}")]
    public async Task<IActionResult> GetByIdDetail(string id)
    {
        var result = await _registrationService.GetByIdRegistrationDetailAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateRegistrationDto createRegistrationDto)
    {
        // TAMAMLANDI-ORTA: Null check eksik - Gerekli kontrol eklendi.
        if (createRegistrationDto is null)
            return BadRequest(ConstantsMessages.RegistrationNotNullMessage);

        // TAMAMLANDI-ORTA: Tip dönüşüm hatası - Bu dönüşüm hem yapıldığı yer olarak yanlış hem de gereksiz. Bu nedenle kaldırıldı. -> var invalidPrice = (int)createRegistrationDto.Price;

        var result = await _registrationService.CreateAsync(createRegistrationDto);
        
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdatedRegistrationDto updatedRegistrationDto)
    {
        if (updatedRegistrationDto is null)
            return BadRequest(ConstantsMessages.RegistrationNotNullMessage);

        var result = await _registrationService.UpdateAsync(updatedRegistrationDto);

        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync([FromBody] DeleteRegistrationDto deleteRegistrationDto)
    {
        var result = await _registrationService.RemoveAsync(deleteRegistrationDto);

        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}