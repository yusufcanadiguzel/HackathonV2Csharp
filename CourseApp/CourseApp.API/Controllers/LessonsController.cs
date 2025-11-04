using CourseApp.EntityLayer.Dto.LessonDto;
using CourseApp.ServiceLayer.Abstract;
using CourseApp.ServiceLayer.Utilities.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CourseApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LessonsController : ControllerBase
{
    private readonly ILessonService _lessonService;
    private readonly IValidator<CreateLessonDto> _createLessonValidator;

    public LessonsController(ILessonService lessonService, IValidator<CreateLessonDto> createLessonValidator)
    {
        _lessonService = lessonService;
        _createLessonValidator = createLessonValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _lessonService.GetAllAsync();
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _lessonService.GetByIdAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("detail")]
    public async Task<IActionResult> GetAllDetail()
    {
        var result = await _lessonService.GetAllLessonDetailAsync();
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("detail/{id}")]
    public async Task<IActionResult> GetByIdDetail(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest(ConstantsMessages.IdNotFoundMessage);

        var result = await _lessonService.GetByIdLessonDetailAsync(id);

        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateLessonDto createLessonDto)
    {
        if (createLessonDto is null)
            return BadRequest(ConstantsMessages.LessonNotNullMessage);

        var validationResult = await _createLessonValidator.ValidateAsync(createLessonDto);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

        // TAMAMLANDI-ORTA: Null check eksik - Gerekli kontrol eklendi.
        var lessonName = createLessonDto.Title;

        // TAMAMLANDI-ORTA: Index out of range - Gerekli validasyon eklendi.
        var firstChar = lessonName[0];
        
        var result = await _lessonService.CreateAsync(createLessonDto);

        if (result.IsSuccess)
        {
            return Ok(result);
        }
        
        return BadRequest(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateLessonDto updateLessonDto)
    {
        if (updateLessonDto is null)
            return BadRequest(ConstantsMessages.LessonNotNullMessage);

        var result = await _lessonService.UpdateAsync(updateLessonDto);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync([FromBody] DeleteLessonDto deleteLessonDto)
    {
        var result = await _lessonService.RemoveAsync(deleteLessonDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}