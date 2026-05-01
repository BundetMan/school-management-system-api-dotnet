
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs;
using SchoolAPI.DTOs.People;
using SchoolAPI.Services.People;

namespace SchoolAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }
    [HttpGet]
    public async Task<ActionResult<PagedResult<StudentDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] bool detail = false)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;
        if (detail) 
        {
            var studentsWithDetails = await _studentService.GetAllWithDetailsAsync(page, pageSize);
            return Ok(studentsWithDetails);
        }
        var students = await _studentService.GetAllAsync(page, pageSize);
        return Ok(students);
    }
    [HttpGet("{code}")]
    public async Task<IActionResult> GetByCode(string code, bool detail = false)
    {
        if (detail)
        {
            var studentDetail = await _studentService.GetByCodeWithDetailsAsync(code);
            if (studentDetail == null) return NotFound();
            return Ok(studentDetail.Adapt<StudentDetailDto>());
        }
        else
        {
            var student = await _studentService.GetCodeAsync(code);
            if (student == null) return NotFound();
            return Ok(student.Adapt<StudentDto>());
        }
    }
    [HttpGet("search")]
    public async Task<IActionResult> Search(
        string? code = null,
        string? latinName = null,
        string? fullName = null,
        int page = 1,
        int pageSize = 10
        )
    {
        var result = await _studentService.SearchAsync(code, latinName, fullName, page, pageSize);
        return Ok(result);
    }
    [HttpGet("search/detail")]
    public async Task<IActionResult> SearchWithDetails(
        string? code = null,
        string? latinName = null,
        string? fullName = null,
        int page = 1,
        int pageSize = 10
        )
    {
        var result = await _studentService.SearchWithDetailsAsync(code, latinName, fullName, page, pageSize);
        return Ok(result);
    }
    [HttpPost]
    public async Task<ActionResult<StudentDetailDto>> Create(StudentCreateDto dto)
    {
        var createdStudent = await _studentService.RegisterStudentAsync(dto);
        if (createdStudent == null) return Conflict(new { Message = "Failed to register student" });
        var dtoResult = createdStudent.Adapt<StudentDetailDto>();
        return CreatedAtAction(nameof(GetByCode), new { createdStudent.Code }, dtoResult);
    }
   
    [HttpPut("{code}")]
    public async Task<IActionResult> Update(string code, StudentUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var updatedStudent = await _studentService.UpdateStudentAsync(code, dto); 
        if (updatedStudent is null) return NotFound($"Student with code '{code}' not found."); 
        return Ok(updatedStudent);
    }
    //[Authorize(Roles = "Admin")]
    [HttpPut("{code}/detail")]
    public async Task<IActionResult> UpdateWithDetails(string code, StudentUpdateDetailDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var updatedStudent = await _studentService.UpdateStudentWithDetailsAsync(code, dto);
        if (updatedStudent is null) return NotFound($"Student with code '{code}' not found.");
        return Ok(updatedStudent);
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("{code}")]
    public async Task<IActionResult> Delete(string code)
    {
        var result = await _studentService.DeleteStudentAsync(code);
        if (result == null) return NotFound($"Student with code '{code}' not found.");
        if (result == false) return Conflict(new { Message = "Failed to delete student" });
        return NoContent();
    }
    [HttpDelete("{code}/softDelete")]
    public async Task<IActionResult> SoftDelete(string code)
    {
        var result = await _studentService.SoftDeleteAsync(code);
        if (result == null) return NotFound($"Student with code '{code}' not found.");
        if (result == false) return Conflict(new { Message = "Failed to soft delete student" });
        return NoContent();
    }
}