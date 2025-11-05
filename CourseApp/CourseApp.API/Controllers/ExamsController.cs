using CourseApp.EntityLayer.Dto.ExamDto;
using CourseApp.ServiceLayer.Abstract;
using CourseApp.ServiceLayer.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;

namespace CourseApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExamsController : ControllerBase
{
    private readonly IExamService _examService;

    public ExamsController(IExamService examService)
    {
        _examService = examService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        // TAMAMLANDI-ZOR: N+1 Problemi - Her exam için ayrı sorgu - Kod güncellendi.
        var result = await _examService.GetAllAsync();

        if (result.IsSuccess)
        {
            // TAMAMLANDI-ORTA: Null reference - Business katmanında null olması durumunda ErrorDataResult dönülüyor
            //var exams = result.Data.ToList();
            // TAMAMLANDI-ZOR: N+1 - Her exam için ayrı sorgu (örnek - gerçek implementasyon service layer'da olabilir) - Dead code olduğu için kaldırıldı.
            //foreach (var exam in exams)
            //{
            //    // TAMAMLANDI: Her exam için ayrı sorgu atılıyor - Include kullanılmamalıydı - Dead code olduğu için kaldırıldı.
            //    // var details = await _examService.GetByIdAsync(exam.Id);
            //}
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _examService.GetByIdAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateExamDto createExamDto)
    {
        if (createExamDto is null)
            return BadRequest(ConstantsMessages.ExamNotNullMessage);

        var result = await _examService.CreateAsync(createExamDto);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateExamDto updateExamDto)
    {
        var result = await _examService.UpdateAsync(updateExamDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteExamDto deleteExamDto)
    {
        if (deleteExamDto is null)
            return BadRequest(ConstantsMessages.ExamNotNullMessage);

        var result = await _examService.RemoveAsync(deleteExamDto);

        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}