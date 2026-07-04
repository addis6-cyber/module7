//module 6 exercise 2
using Microsoft.AspNetCore.Mvc;
using TmsApi.Dtos;
using TmsApi.Services;

namespace TmsApi.Controllers;

[ApiController]
[Route("api/courses")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    // Get a single course by Id
    [HttpGet("{id:int}", Name = nameof(GetCourseById))]
    public async Task<IActionResult> GetCourseById(
        int id,
        CancellationToken ct)
    {
        var course = await _courseService.GetByIdAsync(id, ct);

        if (course == null)
        {
            return NotFound();
        }

        return Ok(course);
    }

    // Create a new course
    [HttpPost]
    public async Task<IActionResult> CreateCourse(
        CreateCourseRequest request,
        CancellationToken ct)
    {
         // Check whether the course code already exists
    if (await _courseService.CodeExistsAsync(request.Code, ct))
    {
        return Conflict(new ProblemDetails
        {
            //module 6 exercise 3
            Title = "Course code already exists",
            Detail = $"A course with code '{request.Code}' already exists.",
            Status = StatusCodes.Status409Conflict
        });
    }

    var result = await _courseService.CreateAsync(request, ct);

    return CreatedAtAction(
        nameof(GetCourseById),
        new { id = result.Id },
        result);
}
}