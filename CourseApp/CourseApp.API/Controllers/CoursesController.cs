using CourseApp.EntityLayer.Dto.CourseDto;
using CourseApp.ServiceLayer.Abstract;
using CourseApp.ServiceLayer.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;

namespace CourseApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _courseService.GetAllAsync();
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute(Name = "id")]string id)
    {
        if (id is null)
            return BadRequest(ConstantsMessages.IdNotFoundMessage);

        var result = await _courseService.GetByIdAsync(id);

        // TAMAMLANDI-ORTA: Null reference - Business katmanında null olması durumunda ErrorDataResult dönülüyor
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("detail")]
    public async Task<IActionResult> GetAllDetail()
    {
        var result = await _courseService.GetAllCourseDetail();
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCourseDto createCourseDto)
    {
        if (createCourseDto is null)
            return BadRequest(ConstantsMessages.CourseNameNotNullMessage);

        // TAMAMLANDI-ORTA: Null check eksik - Gerekli kontrol eklendi
        var courseName = createCourseDto.CourseName;

        // TAMAMLANDI-ORTA: Array index out of range - FluentValidation ile otomatik validasyon yapılarak hata durumunda ilgili status kodu dönülüyor
        var firstChar = courseName[0];
        
        var result = await _courseService.CreateAsync(createCourseDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        
        return BadRequest(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateCourseDto updateCourseDto)
    {
        var result = await _courseService.Update(updateCourseDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteCourseDto deleteCourseDto)
    {
        var result = await _courseService.Remove(deleteCourseDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
