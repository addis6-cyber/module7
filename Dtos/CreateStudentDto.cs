using System.ComponentModel.DataAnnotations;

namespace TmsApi.Models;

public class CreateStudentDto
{
    [Required]
    [MaxLength(20)]
    public string RegistrationNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(10, ErrorMessage = "Name cannot be more than 10 characters.")]
    public string Name { get; set; } = string.Empty;

    [Range(0, 4)]
    public decimal GPA { get; set; }

    public bool IsActive { get; set; }
}