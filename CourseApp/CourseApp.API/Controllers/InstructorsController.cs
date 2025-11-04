using CourseApp.EntityLayer.Dto.InstructorDto;
using CourseApp.ServiceLayer.Abstract;
using CourseApp.ServiceLayer.Utilities.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CourseApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstructorsController : ControllerBase
{
    private readonly IInstructorService _instructorService;
    private readonly IValidator<CreatedInstructorDto> _createdInstructorValidator;

    public InstructorsController(IInstructorService instructorService, IValidator<CreatedInstructorDto> createdInstructorValidator)
    {
        _instructorService = instructorService;
        _createdInstructorValidator = createdInstructorValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _instructorService.GetAllAsync();
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _instructorService.GetByIdAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreatedInstructorDto createdInstructorDto)
    {
        // TAMAMLANDI-ORTA: Null check eksik - Gerekli kontrol eklendi
        if (createdInstructorDto is null)
            return BadRequest(ConstantsMessages.InstructorNotNullMessage);

        var validationResult = await _createdInstructorValidator.ValidateAsync(createdInstructorDto);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

        // TAMAMLANDI-ORTA: Gerekli validasyon eklendi.
        var instructorName = createdInstructorDto.Name;

        // TAMAMLANDI-ORTA: Gerekli validasyon eklendi.
        var firstChar = instructorName[0]; // IndexOutOfRangeException riski

        // TAMAMLANDI-ORTA: Tip dönüşüm hatası - İstenilen işlem için gerekli property(age) mevcut olmadığı için kaldırıldı.
        
        var result = await _instructorService.CreateAsync(createdInstructorDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdatedInstructorDto updatedInstructorDto)
    {
        var result = await _instructorService.Update(updatedInstructorDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeletedInstructorDto deletedInstructorDto)
    {
        var result = await _instructorService.Remove(deletedInstructorDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
