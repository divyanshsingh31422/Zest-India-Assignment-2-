using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Api.Models;

public class Student
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Range(18, 100)]
    public int Age { get; set; }

    [Required]
    [StringLength(50)]
    public string Course { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
