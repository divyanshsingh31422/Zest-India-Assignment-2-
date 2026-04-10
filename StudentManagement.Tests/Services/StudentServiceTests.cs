using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using StudentManagement.Api.Models;
using StudentManagement.Api.Repositories;
using StudentManagement.Api.Services;
using Xunit;

namespace StudentManagement.Tests.Services;

public class StudentServiceTests
{
    private readonly Mock<IStudentRepository> _mockStudentRepository;
    private readonly Mock<ILogger<StudentService>> _mockLogger;
    private readonly StudentService _studentService;

    public StudentServiceTests()
    {
        _mockStudentRepository = new Mock<IStudentRepository>();
        _mockLogger = new Mock<ILogger<StudentService>>();
        _studentService = new StudentService(_mockStudentRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllStudentsAsync_ShouldReturnAllStudents()
    {
        // Arrange
        var students = new List<Student>
        {
            new Student { Id = 1, Name = "John Doe", Email = "john@example.com", Age = 20, Course = "Computer Science" },
            new Student { Id = 2, Name = "Jane Smith", Email = "jane@example.com", Age = 22, Course = "Mathematics" }
        };

        _mockStudentRepository.Setup(repo => repo.GetAllStudentsAsync())
            .ReturnsAsync(students);

        // Act
        var result = await _studentService.GetAllStudentsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockStudentRepository.Verify(repo => repo.GetAllStudentsAsync(), Times.Once);
    }

    [Fact]
    public async Task GetStudentByIdAsync_WithValidId_ShouldReturnStudent()
    {
        // Arrange
        var student = new Student { Id = 1, Name = "John Doe", Email = "john@example.com", Age = 20, Course = "Computer Science" };

        _mockStudentRepository.Setup(repo => repo.GetStudentByIdAsync(1))
            .ReturnsAsync(student);

        // Act
        var result = await _studentService.GetStudentByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("John Doe", result.Name);
        _mockStudentRepository.Verify(repo => repo.GetStudentByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetStudentByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        _mockStudentRepository.Setup(repo => repo.GetStudentByIdAsync(999))
            .ReturnsAsync((Student?)null);

        // Act
        var result = await _studentService.GetStudentByIdAsync(999);

        // Assert
        Assert.Null(result);
        _mockStudentRepository.Verify(repo => repo.GetStudentByIdAsync(999), Times.Once);
    }

    [Fact]
    public async Task CreateStudentAsync_WithValidData_ShouldCreateStudent()
    {
        // Arrange
        var studentDto = new StudentCreateDto
        {
            Name = "John Doe",
            Email = "john@example.com",
            Age = 20,
            Course = "Computer Science"
        };

        var createdStudent = new Student
        {
            Id = 1,
            Name = studentDto.Name,
            Email = studentDto.Email,
            Age = studentDto.Age,
            Course = studentDto.Course,
            CreatedDate = DateTime.UtcNow
        };

        _mockStudentRepository.Setup(repo => repo.EmailExistsAsync(studentDto.Email))
            .ReturnsAsync(false);

        _mockStudentRepository.Setup(repo => repo.CreateStudentAsync(It.IsAny<Student>()))
            .ReturnsAsync(createdStudent);

        // Act
        var result = await _studentService.CreateStudentAsync(studentDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(studentDto.Name, result.Name);
        Assert.Equal(studentDto.Email, result.Email);
        _mockStudentRepository.Verify(repo => repo.EmailExistsAsync(studentDto.Email), Times.Once);
        _mockStudentRepository.Verify(repo => repo.CreateStudentAsync(It.IsAny<Student>()), Times.Once);
    }

    [Fact]
    public async Task CreateStudentAsync_WithDuplicateEmail_ShouldThrowException()
    {
        // Arrange
        var studentDto = new StudentCreateDto
        {
            Name = "John Doe",
            Email = "john@example.com",
            Age = 20,
            Course = "Computer Science"
        };

        _mockStudentRepository.Setup(repo => repo.EmailExistsAsync(studentDto.Email))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _studentService.CreateStudentAsync(studentDto));

        Assert.Contains("already registered", exception.Message);
        _mockStudentRepository.Verify(repo => repo.EmailExistsAsync(studentDto.Email), Times.Once);
        _mockStudentRepository.Verify(repo => repo.CreateStudentAsync(It.IsAny<Student>()), Times.Never);
    }

    [Fact]
    public async Task UpdateStudentAsync_WithValidData_ShouldUpdateStudent()
    {
        // Arrange
        var studentDto = new StudentUpdateDto
        {
            Name = "John Updated",
            Email = "johnupdated@example.com",
            Age = 21,
            Course = "Updated Computer Science"
        };

        var updatedStudent = new Student
        {
            Id = 1,
            Name = studentDto.Name,
            Email = studentDto.Email,
            Age = studentDto.Age,
            Course = studentDto.Course,
            CreatedDate = DateTime.UtcNow
        };

        _mockStudentRepository.Setup(repo => repo.StudentExistsAsync(1))
            .ReturnsAsync(true);

        _mockStudentRepository.Setup(repo => repo.EmailExistsAsync(studentDto.Email, 1))
            .ReturnsAsync(false);

        _mockStudentRepository.Setup(repo => repo.UpdateStudentAsync(It.IsAny<Student>()))
            .ReturnsAsync(updatedStudent);

        // Act
        var result = await _studentService.UpdateStudentAsync(1, studentDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(studentDto.Name, result.Name);
        _mockStudentRepository.Verify(repo => repo.StudentExistsAsync(1), Times.Once);
        _mockStudentRepository.Verify(repo => repo.EmailExistsAsync(studentDto.Email, 1), Times.Once);
        _mockStudentRepository.Verify(repo => repo.UpdateStudentAsync(It.IsAny<Student>()), Times.Once);
    }

    [Fact]
    public async Task UpdateStudentAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var studentDto = new StudentUpdateDto
        {
            Name = "John Updated",
            Email = "johnupdated@example.com",
            Age = 21,
            Course = "Updated Computer Science"
        };

        _mockStudentRepository.Setup(repo => repo.StudentExistsAsync(999))
            .ReturnsAsync(false);

        // Act
        var result = await _studentService.UpdateStudentAsync(999, studentDto);

        // Assert
        Assert.Null(result);
        _mockStudentRepository.Verify(repo => repo.StudentExistsAsync(999), Times.Once);
        _mockStudentRepository.Verify(repo => repo.UpdateStudentAsync(It.IsAny<Student>()), Times.Never);
    }

    [Fact]
    public async Task DeleteStudentAsync_WithValidId_ShouldDeleteStudent()
    {
        // Arrange
        _mockStudentRepository.Setup(repo => repo.StudentExistsAsync(1))
            .ReturnsAsync(true);

        _mockStudentRepository.Setup(repo => repo.DeleteStudentAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _studentService.DeleteStudentAsync(1);

        // Assert
        Assert.True(result);
        _mockStudentRepository.Verify(repo => repo.StudentExistsAsync(1), Times.Once);
        _mockStudentRepository.Verify(repo => repo.DeleteStudentAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteStudentAsync_WithInvalidId_ShouldReturnFalse()
    {
        // Arrange
        _mockStudentRepository.Setup(repo => repo.StudentExistsAsync(999))
            .ReturnsAsync(false);

        // Act
        var result = await _studentService.DeleteStudentAsync(999);

        // Assert
        Assert.False(result);
        _mockStudentRepository.Verify(repo => repo.StudentExistsAsync(999), Times.Once);
        _mockStudentRepository.Verify(repo => repo.DeleteStudentAsync(999), Times.Never);
    }
}
