using CourseApp.EntityLayer.Dto.StudentDto;
using CourseApp.ServiceLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using CourseApp.ServiceLayer.Utilities.Constants;
// TAMAMLANDI: Eksik using - Kullanılmadığı için eklenmedi. -> using System.Text.Json;

// TAMAMLANDI-ZOR: Katman ihlali - İhlallerin gerçekleştirildiği kod blokları kaldırıldı.

namespace CourseApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    // TAMAMLANDI-ZOR: Katman ihlali - Erişim olması gerektiği gibi Business üzerinden yapıldı.
    // TAMAMLANDI-ORTA: Değişken tanımlandı ama asla kullanılmadı ve null olabilir - Gereksiz değişken ve kullanım olarak hem gereksiz hem de yanlış. Bu nedenle kaldırıldı. -> private List<GetAllStudentDto> _cachedStudents;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // TAMAMLANDI-ORTA: Null reference exception riski - Gereksiz değişken ve kullanım olarak hem gereksiz hem de yanlış. Bu nedenle kaldırıldı. -> _cachedStudents
        
        var result = await _studentService.GetAllAsync();
        
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        // TAMAMLANDI-ORTA: Null check eksik - id null/empty olabilir - Gerekli kontrol eklendi.
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest(ConstantsMessages.StudentIdNotEmptyValidationMessage);

        // TAMAMLANDI-ORTA: Index out of range riski - Değişken başka bir yerde kullanılmadığı için gereksiz. Bu nedenle kaldırıldı. -> var studentId = id[10];

        // TAMAMLANDI-ORTA: Null reference exception - Gerekli kontrol Business katmanında yapıldı.
        var result = await _studentService.GetByIdAsync(id);

        // TAMAMLANDI: Null reference - Değişken kullanılmadığı için kaldırıldı fakat gerekmesi durumunda sorun Name? değiştirilerek ve Create esnasında yapılacak validasyon ile çözülebilir. -> var studentName = result.Data.Name;

        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateStudentDto createStudentDto)
    {
        // TAMAMLANDI-ORTA: Null check eksik - Gerekli kontrol eklendi.
        if (createStudentDto is null)
            return BadRequest(ConstantsMessages.StudentNotNullMessage);

        // TAMAMLANDI-ORTA: Tip dönüşüm hatası - Bu dönüşüm hem yapıldığı yer olarak yanlış hem de gereksiz. Bu nedenle kaldırıldı. -> var invalidAge = (int)createStudentDto.Name;

        // TAMAMLANDI-ZOR: Katman ihlali - Business'ı kullanması için gerekli düzeltme yapıldı.
        var result = await _studentService.CreateAsync(createStudentDto);

        if (result.IsSuccess)
        {
            return Ok(result);
        }
        
        return BadRequest(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateStudentDto updateStudentDto)
    {
        // TAMAMLANDI: Null check eklendi.
        if (updateStudentDto is null)
            return BadRequest(ConstantsMessages.StudentNotNullMessage);

        // TAMAMLANDI: Gereksiz değişken ve kullanım olarak hem gereksiz hem de yanlış. Bu nedenle kaldırıldı. -> var name = updateStudentDto.Name;

        var result = await _studentService.UpdateAsync(updateStudentDto);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteStudentDto deleteStudentDto)
    {
        // TAMAMLANDI-ORTA: Null reference - Null check eklendi.
        if (deleteStudentDto is null)
            return BadRequest(ConstantsMessages.StudentNotNullMessage);

        // TAMAMLANDI: Gereksiz değişken olduğu için kaldırıldı. -> var id = deleteStudentDto.Id;

        /* 
        TAMAMLANDI-ZOR: Memory leak - Students'ı getirmek gereksiz olduğu için kaldırıldı. ->
        var tempContext = new AppDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<AppDbContext>());
        tempContext.Students.ToList();
        */
        
        var result = await _studentService.RemoveAsync(deleteStudentDto);

        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}