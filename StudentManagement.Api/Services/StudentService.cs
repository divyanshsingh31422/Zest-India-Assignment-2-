using StudentManagement.Api.Models;
using StudentManagement.Api.Repositories;

namespace StudentManagement.Api.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<StudentService> _logger;

    public StudentService(IStudentRepository studentRepository, ILogger<StudentService> logger)
    {
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Student>> GetAllStudentsAsync()
    {
        try
        {
            _logger.LogInformation("Getting all students");
            return await _studentRepository.GetAllStudentsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all students");
            throw;
        }
    }

    public async Task<Student?> GetStudentByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Getting student with ID: {Id}", id);
            var student = await _studentRepository.GetStudentByIdAsync(id);
            
            if (student == null)
            {
                _logger.LogWarning("Student with ID: {Id} not found", id);
            }
            
            return student;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting student with ID: {Id}", id);
            throw;
        }
    }

    public async Task<Student> CreateStudentAsync(StudentCreateDto studentDto)
    {
        try
        {
            _logger.LogInformation("Creating new student: {Name}", studentDto.Name);

            // Check if email already exists
            if (await _studentRepository.EmailExistsAsync(studentDto.Email))
            {
                _logger.LogWarning("Email already exists: {Email}", studentDto.Email);
                throw new InvalidOperationException($"Email '{studentDto.Email}' is already registered");
            }

            var student = new Student
            {
                Name = studentDto.Name,
                Email = studentDto.Email,
                Age = studentDto.Age,
                Course = studentDto.Course,
                CreatedDate = DateTime.UtcNow
            };

            var createdStudent = await _studentRepository.CreateStudentAsync(student);
            _logger.LogInformation("Successfully created student with ID: {Id}", createdStudent.Id);
            
            return createdStudent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating student: {Name}", studentDto.Name);
            throw;
        }
    }

    public async Task<Student?> UpdateStudentAsync(int id, StudentUpdateDto studentDto)
    {
        try
        {
            _logger.LogInformation("Updating student with ID: {Id}", id);

            // Check if student exists
            if (!await _studentRepository.StudentExistsAsync(id))
            {
                _logger.LogWarning("Student with ID: {Id} not found", id);
                return null;
            }

            // Check if email already exists for another student
            if (await _studentRepository.EmailExistsAsync(studentDto.Email, id))
            {
                _logger.LogWarning("Email already exists for another student: {Email}", studentDto.Email);
                throw new InvalidOperationException($"Email '{studentDto.Email}' is already registered to another student");
            }

            var student = new Student
            {
                Id = id,
                Name = studentDto.Name,
                Email = studentDto.Email,
                Age = studentDto.Age,
                Course = studentDto.Course,
                CreatedDate = DateTime.UtcNow // Keep original creation date or update as needed
            };

            var updatedStudent = await _studentRepository.UpdateStudentAsync(student);
            
            if (updatedStudent != null)
            {
                _logger.LogInformation("Successfully updated student with ID: {Id}", id);
            }
            
            return updatedStudent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating student with ID: {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteStudentAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting student with ID: {Id}", id);

            // Check if student exists
            if (!await _studentRepository.StudentExistsAsync(id))
            {
                _logger.LogWarning("Student with ID: {Id} not found", id);
                return false;
            }

            var result = await _studentRepository.DeleteStudentAsync(id);
            
            if (result)
            {
                _logger.LogInformation("Successfully deleted student with ID: {Id}", id);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting student with ID: {Id}", id);
            throw;
        }
    }
}
