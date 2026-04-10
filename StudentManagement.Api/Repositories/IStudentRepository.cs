using StudentManagement.Api.Models;

namespace StudentManagement.Api.Repositories;

public interface IStudentRepository
{
    Task<IEnumerable<Student>> GetAllStudentsAsync();
    Task<Student?> GetStudentByIdAsync(int id);
    Task<Student> CreateStudentAsync(Student student);
    Task<Student?> UpdateStudentAsync(Student student);
    Task<bool> DeleteStudentAsync(int id);
    Task<bool> StudentExistsAsync(int id);
    Task<bool> EmailExistsAsync(string email, int? excludeId = null);
}
