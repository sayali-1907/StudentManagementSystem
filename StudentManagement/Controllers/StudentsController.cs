using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.DTOs;
using StudentManagement.Services;

namespace StudentManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    /// <summary>Get all students</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentResponseDto>>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var students = await _studentService.GetAllStudentsAsync();
        return Ok(ApiResponse<IEnumerable<StudentResponseDto>>.Ok(students, "Students retrieved successfully"));
    }

    /// <summary>Get a student by ID</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<StudentResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        if (student == null)
            return NotFound(ApiResponse<StudentResponseDto>.Fail($"Student with ID {id} not found."));

        return Ok(ApiResponse<StudentResponseDto>.Ok(student));
    }

    /// <summary>Create a new student</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<StudentResponseDto>), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateStudentDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Validation failed"));

        var student = await _studentService.CreateStudentAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = student.Id },
            ApiResponse<StudentResponseDto>.Ok(student, "Student created successfully"));
    }

    /// <summary>Update an existing student</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<StudentResponseDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStudentDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Validation failed"));

        var student = await _studentService.UpdateStudentAsync(id, dto);
        if (student == null)
            return NotFound(ApiResponse<StudentResponseDto>.Fail($"Student with ID {id} not found."));

        return Ok(ApiResponse<StudentResponseDto>.Ok(student, "Student updated successfully"));
    }

    /// <summary>Delete a student</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _studentService.DeleteStudentAsync(id);
        if (!deleted)
            return NotFound(ApiResponse<object>.Fail($"Student with ID {id} not found."));

        return Ok(ApiResponse<object>.Ok(null!, "Student deleted successfully"));
    }
}
