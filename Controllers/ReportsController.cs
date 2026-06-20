using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TmsApi.Data;

namespace TmsApi.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly TmsDbContext _context;

    public ReportsController(TmsDbContext context)
    {
        _context = context;
    }


    [HttpGet("student-enrollments")]
public IActionResult StudentEnrollments()
{
    var result = _context.Students
        .Select(student => new
        {
            student.Name,
            CourseCount = student.Enrollments.Count()
        })
        .ToList();

    return Ok(result);
}

[HttpGet("high-gpa")]
public IActionResult HighGpaStudents()
{
    var result = _context.Students
        .Where(s => s.GPA > 3.0m)
        .Select(s => new
        {
            s.Name,
            s.GPA
        })
        .ToList();

    return Ok(result);
}

[HttpGet("course-enrollments")]
public IActionResult CourseEnrollments()
{
    var result = _context.Courses
        .Select(course => new
        {
            course.Code,
            course.Title,
            StudentCount = course.Enrollments.Count()
        })
        .ToList();

    return Ok(result);
}

[HttpGet("active-students")]
public IActionResult ActiveStudents()
{
    var result = _context.Students
        .Where(s => s.IsActive)
        .Select(s => new
        {
            s.RegistrationNumber,
            s.Name,
            s.GPA
        })
        .ToList();

    return Ok(result);
}

[HttpGet("student-courses")]
public IActionResult StudentCourses()
{
    var result = _context.Students
        .Select(student => new
        {
            student.Name,
            Courses = student.Enrollments
                .Select(e => e.Course.Code)
                .ToList()
        })
        .ToList();

    return Ok(result);
}
}