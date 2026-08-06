namespace TmsApi.Application.Dtos;

public sealed class EnrollmentResponseDto
{
    public int EnrollmentId { get; set; }
    public int StudentId { get; set; }
    public string CourseCode { get; set; } = string.Empty;
}