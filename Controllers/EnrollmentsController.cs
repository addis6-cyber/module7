using Microsoft.AspNetCore.Mvc;
using TmsApi.Application.Dtos;
using TmsApi.Application.Interfaces;

namespace TmsApi.Controllers;

//[ApiController]
//[Route("api/courses/{courseId}/enrollments")]
[ApiController]
[Route("api/courses/{courseId}/enrollments")]
[Tags("Enrollments")]
[Produces("application/json")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _service;

    public EnrollmentsController(IEnrollmentService service)
    {
        _service = service;
    }

[HttpPost]
[EndpointSummary("Enroll a student into a course")]
[EndpointDescription("Creates an enrollment for a student in the specified course.")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EnrollStudent(
        int courseId,
        CreateEnrollmentRequest request,
        CancellationToken ct)
    {
        var success = await _service.EnrollStudentAsync(
            courseId,
            request,
            ct);

        if (!success)
{
    return BadRequest(new ProblemDetails
    {
        Title = "Enrollment failed",
        Detail = "The student could not be enrolled. The course or student may not exist, or the student is already enrolled.",
        Status = StatusCodes.Status400BadRequest
    });
}

return Ok(new
{
    Message = "Student enrolled successfully."
});
    }
}