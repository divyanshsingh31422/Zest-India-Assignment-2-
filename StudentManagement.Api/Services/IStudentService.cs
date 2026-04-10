using StudentManagement.Api.Models;

namespace StudentManagement.Api.Services;

public interface IStudentService
{
    Task<IEnumerable<Student>> GetAllStudentsAsync();
    Task<Student?> GetStudentByIdAsync(int id);
    Task<Student> CreateStudentAsync(StudentCreateDto studentDto);
    Task<Student?> UpdateStudentAsync(int id, StudentUpdateDto studentDto);
    Task<bool> DeleteStudentAsync(int id);
}
