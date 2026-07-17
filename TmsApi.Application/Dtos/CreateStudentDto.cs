using System.ComponentModel.DataAnnotations;

namespace TmsApi.Application.Dtos;

public class CreateStudentDto
{
    [Required]
    [MaxLength(20)]
    public string RegistrationNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(100, ErrorMessage = "Name cannot be more than 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Range(0, 4)]
    public decimal GPA { get; set; }

    public bool IsActive { get; set; }
}