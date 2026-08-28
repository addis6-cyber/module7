//module 6 exercise 2
using Microsoft.AspNetCore.Mvc;
using TmsApi.Application.Dtos;
using MediatR;
using TmsApi.Application.Courses.Commands;
using TmsApi.Application.Courses.Queries;
using Microsoft.AspNetCore.Authorization;
using TmsApi.Infrastructure.Data;

namespace TmsApi.Controllers;

//[ApiController]
//[Route("api/courses")]
[Authorize(Roles = "Instructor,Admin")]
[ApiController]
[Route("api/courses")]
[Tags("Courses")]
[Produces("application/json")]
public class CoursesController : ControllerBase
{
    
    private readonly IMediator _mediator;

    private readonly IAuthorizationService _authorizationService;

    private readonly TmsDbContext _context;
 public CoursesController(
    IMediator mediator,
    IAuthorizationService authorizationService,
    TmsDbContext context)
{
    _mediator = mediator;
    _authorizationService = authorizationService;
    _context = context;
}

    // Get a single course by Id
    //[HttpGet("{id:int}", Name = nameof(GetCourseById))]
[HttpGet("{id:int}", Name = nameof(GetCourseById))]
[EndpointSummary("Get course by ID")]
[EndpointDescription("Returns a single course together with its enrollment count.")]
[ProducesResponseType(typeof(CourseResponseDto), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseById(
        int id,
        CancellationToken ct)
    {
        var course = await _mediator.Send(new GetCourseByIdQuery(id),ct);

    if (course == null)
    {
        return NotFound();
    }

    return Ok(course);
    }

    // Create a new course
    //[HttpPost]
[HttpPost]
[EndpointSummary("Create a course")]
[EndpointDescription("Creates a new course if the course code does not already exist.")]
[ProducesResponseType(typeof(CourseResponseDto), StatusCodes.Status201Created)]
[ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
public async Task<IActionResult> CreateCourse(
    CreateCourseRequest request,
    CancellationToken ct)
{
    var command = new CreateCourseCommand(
        request.Code,
        request.Title,
        request.MaxCapacity);

    var result = await _mediator.Send(command, ct);

    return CreatedAtAction(
        nameof(GetCourseById),
        new { id = result.Id },
        result);
}
    //module 6 session 2
    // Get all courses with pagination

[HttpGet]
[EndpointSummary("Get paged courses")]
[EndpointDescription("Returns a paginated list of courses with optional searching and sorting.")]
[ProducesResponseType(typeof(PagedResponse<CourseResponseDto>), StatusCodes.Status200OK)]

public async Task<IActionResult> GetCourses(
    [FromQuery] PagedRequest request,
    CancellationToken ct)
{
    var result = await _mediator.Send(
        new GetCoursesQuery(request),
        ct);

    return Ok(result);
}

[HttpPut("{id:int}")]
public async Task<IActionResult> UpdateCourse(
    int id,
    [FromBody] UpdateCourseDto dto)
{
    var course = await _context.Courses.FindAsync(id);

    if (course == null)
    {
        return NotFound();
    }

    var authResult = await _authorizationService.AuthorizeAsync(
        User,
        course,
        "CanEditCourse");

    if (!authResult.Succeeded)
    {
        return Forbid();
    }

    course.Title = dto.Title;

    await _context.SaveChangesAsync();

    return NoContent();
}
}