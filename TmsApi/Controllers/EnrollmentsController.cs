using Microsoft.AspNetCore.Mvc;
using TmsApi.Application.Dtos;
using TmsApi.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using TmsApi.Infrastructure.Data;
using Microsoft.AspNetCore.SignalR;
using TmsApi.Hubs;

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
    private readonly TmsDbContext _db;
    private readonly IHubContext<EnrollmentHub> _hub;
    public EnrollmentsController(
    IEnrollmentService service,
    TmsDbContext db, IHubContext<EnrollmentHub> hub)
    {
        _service = service;
        _db = db;
        _hub = hub;
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

    [HttpGet("/api/enrollments")]
    public async Task<IActionResult> GetAllEnrollments(
    CancellationToken ct)
    {
        var rows = await _db.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .Select(e => new
            {
                id = e.Id.ToString(),
                studentId = e.StudentId,
                studentName = e.Student.Name,
                courseId = e.CourseId,
                courseName = e.Course.Title,
                status = e.Grade == null ? "Pending" : "Approved",
                enrolledAt = e.EnrolledAt
            })
            .OrderByDescending(e => e.enrolledAt)
            .ToListAsync(ct);

        return Ok(rows);
    }

    [HttpPost("/api/enrollments/{id}/approve")]
    public async Task<IActionResult> ApproveEnrollment(
    int id,
    CancellationToken ct)
    {
        var enrollment = await _db.Enrollments
            .FirstOrDefaultAsync(e => e.Id == id, ct);

        if (enrollment is null)
        {
            return NotFound();
        }

        enrollment.Grade = 0;
        await _db.SaveChangesAsync(ct);

    await _hub.Clients.All.SendAsync(
    "EnrollmentApproved",
    new
    {
        id = enrollment.Id
    },
    ct);

        return NoContent();
    }


[HttpDelete("/api/enrollments/{id}")]
public async Task<IActionResult> DeleteEnrollment(
    int id,
    CancellationToken ct)
{
    var enrollment = await _db.Enrollments
        .FirstOrDefaultAsync(e => e.Id == id, ct);

    if (enrollment is null)
    {
        return NotFound(new ProblemDetails
        {
            Title = "Enrollment not found",
            Detail = $"Enrollment with id {id} was not found.",
            Status = StatusCodes.Status404NotFound
        });
    }

    _db.Enrollments.Remove(enrollment);

    await _db.SaveChangesAsync(ct);

    return NoContent();
}

}