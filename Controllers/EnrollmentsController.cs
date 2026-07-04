using Microsoft.AspNetCore.Mvc;
using TmsApi.Dtos;
using TmsApi.Services;

namespace TmsApi.Controllers;

[ApiController]
[Route("api/courses/{courseId}/enrollments")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _service;

    public EnrollmentsController(IEnrollmentService service)
    {
        _service = service;
    }

    [HttpPost]
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
            return BadRequest("Enrollment failed.");
        }

        return Ok("Student enrolled successfully.");
    }
}