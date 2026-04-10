using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Models;
using StudentManagement.Api.Services;

namespace StudentManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStudents()
    {
        try
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all students");
            return StatusCode(500, new { message = "An error occurred while retrieving students" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentById(int id)
    {
        try
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            
            if (student == null)
            {
                return NotFound(new { message = $"Student with ID {id} not found" });
            }

            return Ok(student);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving student with ID: {Id}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the student" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] StudentCreateDto studentDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await _studentService.CreateStudentAsync(studentDto);
            return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation while creating student");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating student");
            return StatusCode(500, new { message = "An error occurred while creating the student" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentUpdateDto studentDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await _studentService.UpdateStudentAsync(id, studentDto);
            
            if (student == null)
            {
                return NotFound(new { message = $"Student with ID {id} not found" });
            }

            return Ok(student);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation while updating student");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating student with ID: {Id}", id);
            return StatusCode(500, new { message = "An error occurred while updating the student" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        try
        {
            var result = await _studentService.DeleteStudentAsync(id);
            
            if (!result)
            {
                return NotFound(new { message = $"Student with ID {id} not found" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting student with ID: {Id}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the student" });
        }
    }
}
