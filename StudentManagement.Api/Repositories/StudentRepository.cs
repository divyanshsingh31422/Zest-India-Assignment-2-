using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.Models;

namespace StudentManagement.Api.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly StudentDbContext _context;
    private readonly ILogger<StudentRepository> _logger;

    public StudentRepository(StudentDbContext context, ILogger<StudentRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Student>> GetAllStudentsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all students");
            return await _context.Students
                .OrderBy(s => s.Name)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all students");
            throw;
        }
    }

    public async Task<Student?> GetStudentByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Retrieving student with ID: {Id}", id);
            return await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving student with ID: {Id}", id);
            throw;
        }
    }

    public async Task<Student> CreateStudentAsync(Student student)
    {
        try
        {
            _logger.LogInformation("Creating new student: {Name}", student.Name);
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully created student with ID: {Id}", student.Id);
            return student;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating student: {Name}", student.Name);
            throw;
        }
    }

    public async Task<Student?> UpdateStudentAsync(Student student)
    {
        try
        {
            _logger.LogInformation("Updating student with ID: {Id}", student.Id);
            var existingStudent = await _context.Students.FindAsync(student.Id);
            if (existingStudent == null)
            {
                _logger.LogWarning("Student with ID: {Id} not found for update", student.Id);
                return null;
            }

            existingStudent.Name = student.Name;
            existingStudent.Email = student.Email;
            existingStudent.Age = student.Age;
            existingStudent.Course = student.Course;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully updated student with ID: {Id}", student.Id);
            return existingStudent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating student with ID: {Id}", student.Id);
            throw;
        }
    }

    public async Task<bool> DeleteStudentAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting student with ID: {Id}", id);
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                _logger.LogWarning("Student with ID: {Id} not found for deletion", id);
                return false;
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully deleted student with ID: {Id}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting student with ID: {Id}", id);
            throw;
        }
    }

    public async Task<bool> StudentExistsAsync(int id)
    {
        try
        {
            return await _context.Students.AnyAsync(s => s.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if student exists with ID: {Id}", id);
            throw;
        }
    }

    public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
    {
        try
        {
            var query = _context.Students.Where(s => s.Email == email);
            if (excludeId.HasValue)
            {
                query = query.Where(s => s.Id != excludeId.Value);
            }
            return await query.AnyAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if email exists: {Email}", email);
            throw;
        }
    }
}
