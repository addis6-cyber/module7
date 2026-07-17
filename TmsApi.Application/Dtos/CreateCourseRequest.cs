using System.ComponentModel.DataAnnotations;

namespace TmsApi.Application.Dtos;
public class CreateCourseRequest
{
    [Required]
    [RegularExpression(
        @"^[A-Z]{2,3}-\d{3}$",
        ErrorMessage = "Code must be in the format ABC-123.")]
    public string Code { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Range(1, 200)]
    public int MaxCapacity { get; set; }
}