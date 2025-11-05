using CourseApp.EntityLayer.Dto.ExamResultDto;
using CourseApp.ServiceLayer.Abstract;
using CourseApp.ServiceLayer.Utilities.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CourseApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExamResultsController : ControllerBase
{
    private readonly IExamResultService _examResultService;

    public ExamResultsController(IExamResultService examResultService)
    {
        _examResultService = examResultService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // TAMAMLANDI-ZOR: N+1 Problemi - Her examResult için ayrı sorgu
        var result = await _examResultService.GetAllAsync();

        /* TAMAMLANDI: Dead code olduğu için kaldırıldı.
        // TAMAMLANDI-ORTA: Null reference - Gerekli null check manager'a eklendi.
        if (result.IsSuccess && result.Data != null)
        {
            // TAMAMLANDI-ZOR: N+1 - Her examResult için detay çekiliyor - Dead code olduğu için kaldırıldı.
            var examResults = result.Data.ToList();
            foreach (var examResult in examResults)
            {
                // Her examResult için ayrı sorgu
                var detail = await _examResultService.GetByIdExamResultDetailAsync(examResult.Id);
            }
            return Ok(result);
        }
        */
        
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _examResultService.GetByIdAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("detail")]
    public async Task<IActionResult> GetAllDetail()
    {
        var result = await _examResultService.GetAllExamResultDetailAsync();
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("detail/{id}")]
    public async Task<IActionResult> GetByIdDetail(string id)
    {
        var result = await _examResultService.GetByIdExamResultDetailAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateExamResultDto createExamResultDto)
    {
        if (createExamResultDto is null)
            return BadRequest(ConstantsMessages.ExamResultNotNullMessage);

        var result = await _examResultService.CreateAsync(createExamResultDto);

        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateExamResultDto updateExamResultDto)
    {
        var result = await _examResultService.UpdateAsync(updateExamResultDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteExamResultDto deleteExamResultDto)
    {
        var result = await _examResultService.RemoveAsync(deleteExamResultDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}